using System;

namespace FSMSimple.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            Example example = new Example();
            example.Show();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
