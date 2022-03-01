using System.Net;
using System.Net.Sockets;
using System.Text;

Console.WriteLine("Welcome to sky-watcher motor in the middle");
Console.WriteLine("The goal here is to learn more about the sky-watcher protocol");

int port = 11880;

var address = IPAddress.Parse("192.168.39.111");
var clientEndpoint = new IPEndPoint(address, port);
var client = new UdpClient();
client.Connect(clientEndpoint);

var server = new UdpClient(port);
var serverEndpoint = new IPEndPoint(IPAddress.Any, port);

try
{
    while (true)
    {
        byte[] bytes = server.Receive(ref serverEndpoint);
        string command = Encoding.ASCII.GetString(bytes, 0, bytes.Length).Trim();

        if (string.IsNullOrEmpty(command) || command == ":")
        {
            continue;
        }

        byte[] data = client.SendAndReceive(clientEndpoint, bytes);

        if (data == null)
        {
            continue;
        }

        string response = Encoding.ASCII.GetString(data, 0, data.Length);
        server.Send(data, serverEndpoint);

        if(command.StartsWith(":j") || command.StartsWith(":f"))
        {
            continue;
        }
        
        if(command.StartsWith(":G")) 
        {
            Console.WriteLine($"{DateTime.Now.ToLongTimeString()}\t{command}{response}");
        }
    }
}
catch (SocketException e)
{
    Console.WriteLine(e);
}
finally
{
    server.Close();
}
public static class UdpClientExtensions
{
    public static byte[] SendAndReceive(
        this UdpClient client,
        IPEndPoint clientEndpoint,
        byte[] bytes
    )
    {
        byte[] response = null;
        AutoResetEvent wait = new AutoResetEvent(false);

        var result = client.BeginReceive(new AsyncCallback((IAsyncResult ar) =>
        {
            response = client.EndReceive(ar, ref clientEndpoint);
            wait.Set();

        }), null);

        client.Send(bytes);

        wait.WaitOne(500);

        return response;
    }
}