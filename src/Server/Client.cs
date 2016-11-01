using SocketListener;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Server
{
    public class Client : NetConnection
    {
        public Client()
            : base()
        {
        }

        public Client(Socket socket)
            : base(socket)
        {
        }

        public override void HandleMessage(Packet packet)
        {
            Console.WriteLine("Incoming message");
        }
    }
}
