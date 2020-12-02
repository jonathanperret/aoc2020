using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using MoreLinq;

namespace cli
{
    public static class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("day2.txt");

            int validCount = 0;
            foreach (string line in lines)
            {
                var sp = line.Split("- :".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                int min = int.Parse(sp[0]), max = int.Parse(sp[1]);
                char letter = sp[2][0];
                string pass = sp[3];
                int letterCount = pass.Where(c => c == letter).Count();
                bool valid =
                    min <= pass.Length && max <= pass.Length && (
                        pass.ElementAt(min - 1) == letter ^ pass.ElementAt(max - 1) == letter
                    );
                if (valid) validCount++;
                Console.WriteLine($"{min} {max} {letter} {pass} {letterCount} {valid}");
            }

            Console.WriteLine(validCount);
        }
    }
}
