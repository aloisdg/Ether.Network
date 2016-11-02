using System;
using System.Linq;

namespace Ether.Network.Helpers
{
    public static partial class Helper
    {
        private const string CHARACTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        /// <summary>
        /// Generates a random string.
        /// </summary>
        /// <param name="count">Length of the string.</param>
        /// <returns>Generated string</returns>
        public static string GenerateRandomString(int count = 8)
        {
            var random = new Random();

            return new string(
                Enumerable.Repeat(CHARACTERS, count)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
        }
    }
}
