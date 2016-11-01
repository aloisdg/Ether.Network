using System;
using System.Linq;

namespace SocketListener
{
    public static class Helper
    {
        private static string Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static int id = 0;
        public static int GenerateUniqueId()
        {
            return ++id;
        }

        public static string GenerateRandomString()
        {
            var random = new Random();

            return new string(
                            Enumerable.Repeat(Characters, 42)
                                      .Select(s => s[random.Next(s.Length)])
                                      .ToArray());
        }
    }
}
