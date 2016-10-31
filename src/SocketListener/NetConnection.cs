using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SocketListener
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
            this.Socket = acceptedSocket;
        }

        internal void Initialize(Socket acceptedSocket)
        {
            if (this.Socket == null)
            {
                this.Socket = acceptedSocket;
            }
        }

        public abstract void HandleMessage(Packet packet);

        public void Dispose()
        {
        }
    }
}
