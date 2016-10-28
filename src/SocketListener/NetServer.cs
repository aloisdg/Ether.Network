using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace SocketListener
{
    public abstract class NetServer<T> : IDisposable where T : NetConnection, new()
    {
        private static object syncClients = new object();

        private Socket listenSocket;
        private List<NetConnection> clients;
        private Thread listenThread;

        public NetServer()
        {
        }

        public void Start()
        {
            this.Initialize();

            this.listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.listenSocket.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 4444));
            this.listenSocket.Listen(100);

            this.listenThread = new Thread(new ThreadStart(this.ListenSocket));
            this.listenThread.Start();

            this.Idle();
        }

        public void Stop()
        {
        }

        private void ListenSocket()
        {
            while (true)
            {
                if (this.listenSocket.Poll(100, SelectMode.SelectRead))
                {
                    T client = new T();

                    client.Initialize(this.listenSocket.Accept());

                    lock (syncClients)
                        this.clients.Add(client);
                }
            }
        }

        public void Dispose()
        {
            this.listenThread.Join();
            this.listenSocket.Dispose();

            foreach (NetConnection connection in this.clients)
                connection.Dispose();

            this.clients.Clear();
        }

        protected abstract void Initialize();

        protected abstract void Idle();
    }
}
