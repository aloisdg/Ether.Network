using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocketListener
{
    public abstract class NetServer<T> : IDisposable where T : NetConnection, new()
    {
        public NetServer()
        {
        }

        public void Start()
        {
            this.Initialize();

            // Initialize sockets and threads

            this.Idle();
        }

        public void Stop()
        {
        }

        public void Dispose()
        {
        }

        protected abstract void Initialize();

        protected abstract void Idle();
    }
}
