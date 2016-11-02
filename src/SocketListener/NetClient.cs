using System;
using System.Net;
using System.Net.Sockets;

namespace SocketListener
{
    public class NetClient : NetConnection
    {
        private bool isRunning;

        public NetClient()
            : base(new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
        {
            this.isRunning = false;
        }

        public void Connect(string ip, int port)
        {
            if (this.Socket.Connected)
                return;
            this.Socket.Connect(new IPEndPoint(IPAddress.Parse(ip), port));
            this.isRunning = true;
        }

        public void Disconnect()
        {
            if (this.Socket.Connected)
                this.Socket.Dispose();
        }

        public void Run()
        {
            while (this.isRunning)
            {
                if (!this.Socket.Poll(100, SelectMode.SelectRead)) continue;

                try
                {
                    var buffer = new byte[this.Socket.Available];
                    var recievedDataSize = this.Socket.Receive(buffer);

                    if (recievedDataSize < 0)
                        throw new Exception("Disconnected");
                    var recievedPackets = Packet.Split(buffer);

                    foreach (var packet in recievedPackets)
                    {
                        this.HandleMessage(packet);
                        packet.Dispose();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(!this.Socket.Connected ? "Client disconnected" : $"Error: {e.Message}");
                }
            }
        }

        public override void Greetings()
        {
            // not used
            throw new NotImplementedException();
        }

        public override void HandleMessage(Packet packet)
        {
            Console.WriteLine("Incoming message");

            //// RECIEVE PACKET ////

            //int packetSize = packet.Read<int>();
            //Console.WriteLine("==> Packet size: {0}", packetSize);

            var packetHeader = packet.Read<string>();
            Console.WriteLine("==> packet header: {0}", packetHeader);

            var packetContent = packet.Read<string>();

            Console.WriteLine("==> Packet content: {0}", packetContent);

            //// SEND PACKET ////

            var randomString = Helper.GenerateRandomString();

            var newPacket = new Packet();

            newPacket.Write("hello world!");
            newPacket.Write(randomString);

            this.Send(newPacket);
        }
    }
}
