using System;
using System.Linq;

namespace SocketListener
{
    public static class Helper
    {
        private static string Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static int id = 0;

        /// <summary>
        /// Generates an unique Id.
        /// </summary>
        /// <returns></returns>
        public static int GenerateUniqueId()
        {
            return ++id;
        }

        /// <summary>
        /// Generates a random string.
        /// </summary>
        /// <param name="count">Length of the string.</param>
        /// <returns>Generated string</returns>
        public static string GenerateRandomString(int count = 8)
        {
            var random = new Random();

            return new string(
                            Enumerable.Repeat(Characters, count)
                                      .Select(s => s[random.Next(s.Length)])
                                      .ToArray());
        }
    }
}
