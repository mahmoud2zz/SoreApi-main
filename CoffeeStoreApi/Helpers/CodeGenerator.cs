using System;
namespace CoffeeStoreApi.Helpers
{
    public static class CodeGenerator
    {
        public static string Generate4DigitCode()
        {
            return new Random().Next(1000, 10000).ToString();
        }
    }
}

