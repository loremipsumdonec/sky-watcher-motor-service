using SkyWatcherMotorService.Features.SkyWatcher.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SkyWatcherMotorServiceTest.SUT.Features
{
    public class Exploratory
    {
        private bool _messageReceived;
        private string _response;

        public struct UdpState
        {
            public UdpClient u;
            public IPEndPoint e;
        }

        private IList<long> Decode(string packedHexDecimal) 
        {
            List<long> values = new List<long>();

            for (int i = 0; i < packedHexDecimal.Length; i += 2)
            {
                string sub = packedHexDecimal.Substring(i, 2);
                int v = int.Parse(sub, System.Globalization.NumberStyles.AllowHexSpecifier);
                values.Add(v);
            }

            return values;
        }
        
        [Fact]
        public async Task Donec() 
        {
            var manager = new SkyWatcherManager();
            
            using(var client = manager.CreateClient("mount://192.168.39.111:11880/2")) 
            {
                var (total, major, minor, patch) = await client.SendAndReciveSectionsAsync("e");
                string version = $"{major}.{minor}.{patch:X}";

                var cpr = await client.SendAndRecieveAsync("a");

                double factorRadianToStep = cpr / (2 * Math.PI);
                double factorStepToRadian = 2 * Math.PI / cpr;

                var timerInterruptFreq = await client.SendAndRecieveAsync("b");
                var highSpeedRatio = await client.SendAndRecieveAsync("g");

                var position = await client.SendAndRecieveAsync("j") - 0x00800000;
                var positionInRadians = position * factorStepToRadian;
                var angle = positionInRadians * 180 / Math.PI;

                double target = 30 * Math.PI / 180.0;
                long steps = (long)(target * factorRadianToStep);

                await client.SendAsync("G", 1, 2);
                await client.SendAsync("H", steps);
                await client.SendAsync("M", 3500);
                await client.SendAsync("J", 0);
            }
        }

        [Fact]
        public void Connect() 
        {   
            var address = IPAddress.Parse("192.168.39.111");
            var endpoint = new IPEndPoint(address, 11880);
            var client = new UdpClient();
            client.Connect(endpoint);

            UdpState state = new UdpState();
            state.e = endpoint;
            state.u = client;

            client.BeginReceive(new AsyncCallback(ReceiveCallback), state);

            var i = client.Send(Encoding.UTF8.GetBytes(":b1\r"));

            while (!_messageReceived)
            {
                Thread.Sleep(100);
            }

            Assert.NotNull(_response);
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            UdpClient u = ((UdpState)(ar.AsyncState)).u;
            IPEndPoint e = ((UdpState)(ar.AsyncState)).e;

            byte[] receiveBytes = u.EndReceive(ar, ref e);
            string receiveString = Encoding.UTF8.GetString(receiveBytes);

            Console.WriteLine($"Received: {receiveString}");
            _response = receiveString;
            _messageReceived = true;
        }
    }
}
