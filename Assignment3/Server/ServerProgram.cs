
using System.Net.Sockets;
using System.Net;
using System.Text;
using Assignment3.Server;

namespace Assignment3.Server
{
    class ServerProgram
    {
        static void Main(string[] args)
        {
            int port = 5000;

            var server = new EchoServer(port);

            server.Run();
        }
    }
}

