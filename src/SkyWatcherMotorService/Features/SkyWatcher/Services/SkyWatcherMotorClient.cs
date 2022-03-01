using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SkyWatcherMotorService.Features.SkyWatcher.Services
{
    public class SkyWatcherMotorClient
        : ISkyWatcherMotorClient, IDisposable
    {
        private readonly object _door;
        private IPEndPoint _endpoint;
        private readonly int _timeout = 500;
        private readonly int _channel;

        public SkyWatcherMotorClient(int channel, string address, int port, object door) 
        {
            _channel = channel;

            var ip = IPAddress.Parse(address);
            _endpoint = new IPEndPoint(ip, port);

            _door = door;
        }

        public async ValueTask<(long, long, long, long)> SendAndReciveSectionsAsync(string header) 
        {
            var response = await SendCommandAsync($":{header}{_channel}");
            return Decode24(response);
        }

        public async ValueTask<long> SendAndRecieveAsync(string header)
        {
            var response = await SendCommandAsync($":{header}{_channel}");
            return Decode24(response).Item1;
        }

        public async Task SendAsync(string header, long data, int length = 6) 
        {
            if(data == 0 && length == 6)
            {
                var re = await SendCommandAsync($":{header}{_channel}");
                return;
            }

            var response = await SendCommandAsync($":{header}{_channel}{Encode(data, length)}");
        }

        public async ValueTask<string> SendCommandAsync(string command)
        {
            var response = await Task.Run(() =>
            {
                lock(_door)
                {
                    byte[] response = null;

                    var client = new UdpClient();
                    client.Connect(_endpoint);

                    AutoResetEvent wait = new AutoResetEvent(false);

                    var result = client.BeginReceive(new AsyncCallback((IAsyncResult ar) =>
                    {
                        response = client.EndReceive(ar, ref _endpoint);
                        wait.Set();

                    }), null);

                    byte[] data = Encoding.ASCII.GetBytes(command + '\r');
                    int length = client.Send(data);

                    wait.WaitOne(_timeout);

                    return response;
                }
            });

            if(response == null || response.Length == 0) 
            {
                return string.Empty;
            }

            string data = Encoding.ASCII.GetString(response, 0, response.Length);

            if(string.IsNullOrEmpty(data)) 
            {
                return string.Empty;
            }

            return data.Substring(1, data.Length - 2);
        }

        private string Encode(long data, int length) 
        {
            string a = ((int)data & 0xFF).ToString("X").ToUpper();
            string b = (((int)data & 0xFF00) / 256).ToString("X").ToUpper();
            string c = (((int)data & 0xFF0000) / 256 / 256).ToString("X").ToUpper();

            if (a.Length == 1)
                a = "0" + a;
            if (b.Length == 1)
                b = "0" + b;
            if (c.Length == 1)
                c = "0" + c;

            return (a + b + c).Substring(0,length);
        }

        private (long, long, long, long) Decode24(string packedHexDecimal)
        {
            var values = Decode(packedHexDecimal);
            return (values[3], values[0], values[1], values[2]);
        }

        private IList<long> Decode(string packedHexDecimal)
        {
            List<long> values = new List<long>();

            double total = 0;

            if(packedHexDecimal.Length == 3) 
            {
                for (int i = 0; i < packedHexDecimal.Length; i += 1)
                {
                    string sub = packedHexDecimal.Substring(i, 1);
                    int v = int.Parse(sub, System.Globalization.NumberStyles.AllowHexSpecifier);
                    total += v * Math.Pow(16, i);

                    values.Add(v);
                }
            }

            if(packedHexDecimal.Length == 6) 
            {
                for (int i = 0; i < packedHexDecimal.Length; i += 2)
                {
                    string sub = packedHexDecimal.Substring(i, 2);
                    int v = int.Parse(sub, System.Globalization.NumberStyles.AllowHexSpecifier);
                    total += v * Math.Pow(16, i);

                    values.Add(v);
                }
            }

            while(values.Count < 3)
            {
                values.Add(0);
            }

            values.Add((long)total);

            return values;
        }

        public void Dispose()
        {
        }
    }
}
