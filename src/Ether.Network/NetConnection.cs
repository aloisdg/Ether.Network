using Ether.Network.Helpers;
using Ether.Network.Packets;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Ether.Network
{
    public abstract class NetConnection : IDisposable
    {
        public int Id { get; protected set; }

        public Socket Socket { get; private set; }

        protected bool Working { get; set; }

        public NetConnection()
            : this(null)
        {
        }

        public NetConnection(Socket acceptedSocket)
        {
            this.Id = Helper.GenerateUniqueId();
            this.Socket = acceptedSocket;
            this.Working = false;
        }

        internal void Initialize(Socket acceptedSocket)
        {
            if (this.Socket != null) return;
            this.Socket = acceptedSocket;
            this.Greetings();
        }

        public abstract void Greetings();

        public abstract void HandleMessage(NetPacket packet);

        public void Send(NetPacket packet)
        {
            this.Socket.Send(packet.Buffer);
        }

        public void SendTo(NetConnection client, NetPacket packet)
        {
            client.Send(packet);
        }

        public void SendTo(ICollection<NetConnection> clients, NetPacket packet)
        {
            foreach (var client in clients)
                client.Send(packet);
        }

        public void SendToAll(NetPacket packet)
        {
            // TODO
        }

        public void Dispose()
        {
            if (this.Socket == null) return;
            this.Socket.Dispose();
            this.Socket = null;
        }
    }
}
