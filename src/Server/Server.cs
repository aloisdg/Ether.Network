using Ether.Network;
using System;

namespace Server
{
    public class Server : NetServer<Client>
    {
        public Server()
            : base()
        {
        }

        protected override void Initialize()
        {
            // TODO: initialize specific server resources at startup.
        }

        protected override void Idle()
        {
            // TODO: do custom process on main thread.
            while (true)
            {
                Console.ReadKey();
            }
        }
    }
}
