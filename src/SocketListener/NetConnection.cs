using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SocketListener
{
    public abstract class NetConnection : IDisposable
    {
        private Socket socket;

        public NetConnection() 
            : this(null)
        {
        }

        public NetConnection(Socket acceptedSocket)
        {
            this.socket = acceptedSocket;
        }

        internal void Initialize(Socket acceptedSocket)
        {
            if (this.socket == null)
            {
                this.socket = acceptedSocket;
            }
        }

        public abstract void HandleMessage();

        public void Dispose()
        {
        }
    }
}
