using System;

namespace Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "Server";

            using (Server server = new Server())
                server.Start();
        }
    }
}
