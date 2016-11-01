using SocketListener;

namespace Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            NetClient client = new NetClient();

            client.Connect("127.0.0.1", 4444);
            client.Run(); // blocking
        }
    }
}
