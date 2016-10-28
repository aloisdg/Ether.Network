namespace Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (Server server = new Server())
                server.Start();
        }
    }
}
