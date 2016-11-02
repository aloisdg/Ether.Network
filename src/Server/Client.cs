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

        public override void Greetings()
        {
            Packet packet = new Packet();

            packet.Write("Hello World!");
            packet.Write("Hello!");

            this.Send(packet);
        }

        public override void HandleMessage(Packet packet)
        {
            Console.WriteLine("Incoming message");

            string packetHeader = packet.Read<string>();
            string packetContent = packet.Read<string>();

            Console.WriteLine("==> Packet header : {0}", packetHeader);
            Console.WriteLine("==> packet content : {0}", packetContent);

            //// SEND PACKET ////

            string randomString = Helper.GenerateRandomString();

            Packet newPacket = new Packet();

            newPacket.Write("Hello world!");
            newPacket.Write(randomString);

            this.Send(newPacket);
        }
    }
}
