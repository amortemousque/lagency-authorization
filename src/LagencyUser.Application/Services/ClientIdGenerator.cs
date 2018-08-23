using System;
using System.Linq;

namespace LagencyUser.Application.Service
{
    public class ClientIdGenerator
    {
        public ClientIdGenerator()
        {
        }

        private static Random random = new Random();
        public static string Generate(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
