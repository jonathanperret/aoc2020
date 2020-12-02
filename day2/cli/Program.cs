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
                var sp = line.Split(" ");
                var minmax = sp[0].Split("-").Select(int.Parse).ToArray();
                int min = minmax[0], max = minmax[1];
                char letter = sp[1].Substring(0, 1).ToCharArray()[0];
                string pass = sp[2];
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
