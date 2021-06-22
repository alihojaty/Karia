using System;

namespace Karia.Api.Helpers
{
    public static class GenerateRandomNumber
    {
        public static int GetCode()
        {
            var random = new Random();
            return random.Next(100000, 999999);
        }
    }
}