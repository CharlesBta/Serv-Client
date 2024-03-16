using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.Json;
using Microsoft.VisualBasic;


void sendMessage(object clientSocket)
{
    if (clientSocket is Socket socket)
    {
        var endpoint = socket.RemoteEndPoint;
        if (endpoint is not null)
        {
            Console.WriteLine("Client connecté depuis et sur un thread "
                              + endpoint);
            int i = 0;
            Thread.Sleep(2000);
            while (true)
            {
                var json = $"{{\"value\" : \"{i}\"}}";
                var buffer = Encoding.UTF8.GetBytes(json);
                try
                {
                    socket.Send(buffer);
                    Console.WriteLine($"Message envoyé : {json} à {endpoint}");
                    i++;
                }
                catch
                {
                    break;
                }
                Thread.Sleep(1000);
            }
            Console.WriteLine($"Client {endpoint} déconnecté");
        }
    }
}


Socket socket = new Socket(
    AddressFamily.InterNetwork,
    SocketType.Stream,
    ProtocolType.Tcp);

IPEndPoint endpoint = new IPEndPoint(
    IPAddress.Parse("127.0.0.1"),
    2345
);
try
{
    socket.Bind(endpoint);
    socket.Listen();
    Console.Clear();
}
catch
{
    Console.WriteLine("Impossible de démarrer le serveur");
    Environment.Exit(-1);
}

try
{
    while (true)
    {
        var clientSocket = socket.Accept();
        if (true)
        {
            Thread threadClient = new Thread(sendMessage!);
            threadClient.IsBackground = true;
            threadClient.Start(clientSocket);
        }
    }
}
catch
{
    Console.WriteLine("La communication avec le client n'est pas possible");
}
finally
{
    if (socket.Connected)
    {
        socket.Shutdown(SocketShutdown.Both);
    }

    socket.Close();
}