using Ether.Network;
using Ether.Network.Helpers;
using Ether.Network.Packets;
using System;
using System.Net.Sockets;

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
            var packet = new NetPacket();

            packet.Write("Hello World!");
            packet.Write("Hello!");

            this.Send(packet);
        }

        public override void HandleMessage(NetPacket packet)
        {
            Console.WriteLine("Incoming message");

            string packetHeader = packet.Read<string>();
            string packetContent = packet.Read<string>();

            Console.WriteLine("==> Packet header : {0}", packetHeader);
            Console.WriteLine("==> packet content : {0}", packetContent);

            //// SEND PACKET ////

            string randomString = Helper.GenerateRandomString();

            var newPacket = new NetPacket();

            newPacket.Write("Hello world!");
            newPacket.Write(randomString);

            this.Send(newPacket);
        }
    }
}
