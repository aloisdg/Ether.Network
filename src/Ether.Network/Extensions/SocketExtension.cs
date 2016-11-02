using System.Net.Sockets;

namespace Ether.Network.Extensions
{
    public static class SocketExtensions
    {
        /// <summary>
        /// Check if the socket is connected.
        /// </summary>
        /// <param name="socket">Socket</param>
        /// <returns></returns>
        public static bool IsConnected(this Socket socket)
        {
            try
            {
                return !(socket.Poll(1, SelectMode.SelectRead) && socket.Available == 0);
            }
            catch (SocketException)
            {
                return false;
            }
        }
    }
}
