using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

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
            if (this.Socket.Connected == false)
            {
                this.Socket.Connect(new IPEndPoint(IPAddress.Parse(ip), port));
                this.isRunning = true;
            }
        }

        public void Disconnect()
        {
            if (this.Socket.Connected)
            {
                this.Socket.Dispose();
            }
        }

        public void Run()
        {
            while (this.isRunning)
            {
                if (this.Socket.Poll(100, SelectMode.SelectRead))
                {
                    int recievedDataSize = 0;
                    byte[] buffer;

                    try
                    {
                        buffer = new byte[this.Socket.Available];
                        recievedDataSize = this.Socket.Receive(buffer);

                        if (recievedDataSize < 0)
                            throw new Exception("Disconnected");
                        else
                        {
                            Packet[] recievedPackets = Packet.Split(buffer);

                            foreach (Packet packet in recievedPackets)
                            {
                                this.HandleMessage(packet);
                                packet.Dispose();
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        if (this.Socket.Connected == false)
                        {
                            Console.WriteLine("Client disconnected");
                        }
                        else
                        {
                            Console.WriteLine("Error: {0}", e.Message);
                        }
                    }
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

            string packetHeader = packet.Read<string>();
            Console.WriteLine("==> packet header: {0}", packetHeader);

            string packetContent = packet.Read<string>();

            Console.WriteLine("==> Packet content: {0}", packetContent);

            //// SEND PACKET ////

            string randomString = Helper.GenerateRandomString();

            Packet newPacket = new Packet();

            newPacket.Write<string>("hello world!");
            newPacket.Write(randomString);

            this.Send(newPacket);
        }
    }
}
