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
        private Thread handlerThread;

        public Boolean IsRunning { get; private set; }

        public NetServer()
        {
            this.IsRunning = false;
            this.clients = new List<NetConnection>();
        }

        ~NetServer()
        {
            this.Dispose(false);
        }

        public void Start()
        {
            if (this.IsRunning == false)
            {
                this.Initialize();

                this.listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                this.listenSocket.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 4444));
                this.listenSocket.Listen(100);

                this.listenThread = new Thread(this.ListenSocket);
                this.listenThread.Start();

                this.handlerThread = new Thread(this.HandleClients);
                this.handlerThread.Start();

                this.IsRunning = true;

                this.Idle();
            }
            else
                throw new InvalidOperationException("NetServer is already running.");
        }

        public void Stop()
        {
            if (this.IsRunning)
            {
                this.IsRunning = false;
                this.Dispose();
            }
        }

        private void ListenSocket()
        {
            while (this.IsRunning)
            {
                if (this.listenSocket.Poll(100, SelectMode.SelectRead))
                {
                    Console.WriteLine("New client connected");
                    var client = new T();

                    client.Initialize(this.listenSocket.Accept());

                    lock (syncClients)
                        this.clients.Add(client);
                }

                Thread.Sleep(100);
            }
        }

        private void HandleClients()
        {
            var clientsReady = new Queue<NetConnection>();

            try
            {
                while (this.IsRunning)
                {
                    lock (syncClients)
                    {
                        foreach (var client in this.clients)
                        {
                            if (client.Socket.Poll(100, SelectMode.SelectRead))
                                clientsReady.Enqueue(client);
                        }
                    }

                    while (clientsReady.Any())
                    {
                        var client = clientsReady.Dequeue();

                        var recievedDataSize = 0;
                        byte[] buffer;

                        try
                        {
                            buffer = new Byte[client.Socket.Available];
                            recievedDataSize = client.Socket.Receive(buffer);

                            if (recievedDataSize <= 0)
                                throw new Exception("Disconnected");
                            else
                            {
                                var recievedPackets = Packet.Split(buffer);

                                foreach (var packet in recievedPackets)
                                {
                                    client.HandleMessage(packet);
                                    packet.Dispose();
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            if (client.Socket.Connected == false)
                            {
                                Console.WriteLine("Client disconnected");
                                this.RemoveClient(client);
                            }
                            else
                            {
                                Console.WriteLine("Error: {0}\n{1}", e.Message, e.StackTrace);
                            }
                        }
                    }
                    
                    Thread.Sleep(50);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected error: {0}. StackTrace:{1}{2}", 
                    e.Message, Environment.NewLine, e.StackTrace);
            }
        }

        public void RemoveClient(NetConnection client)
        {
            lock (syncClients)
            {
                var _clientToRemove = this.clients.Find(item => { return (item is NetConnection && (item as NetConnection) == client); });

                this.clients.Remove(_clientToRemove);
            }
        }

        protected abstract void Initialize();

        protected abstract void Idle();

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.listenThread.Join();
                    this.handlerThread.Join();

                    this.listenThread = null;
                    this.handlerThread = null;

                    this.listenSocket.Dispose();
                    
                    foreach (var connection in this.clients)
                        connection.Dispose();

                    this.clients.Clear();
                }

                disposedValue = true;
            }
        }
        
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
