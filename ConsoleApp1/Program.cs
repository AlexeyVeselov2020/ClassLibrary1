using System;
using QRLibrary;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            string request = Console.ReadLine();
            Console.WriteLine(Codding.EncodeRequest(request));
        }
    }
}
