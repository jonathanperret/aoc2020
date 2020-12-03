using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using MoreLinq;
using Sprache;

namespace cli
{
    public static class Program
    {
        public static long Product(this IEnumerable<long> seq)
        {
            return seq.Aggregate((a, b) => a * b);
        }

        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("day3.txt");

            (int x, int y)[] slopes = { (1, 1), (3, 1), (5, 1), (7, 1), (1, 2) };

            long total = lines.Select((line, y) =>
                    slopes.Select(s => y % s.y == 0 && line[s.x * y / s.y % line.Length] == '#' ? 1L : 0)
                )
                .Transpose()
                .Select(s => s.Sum())
                .Product();

            Console.WriteLine(total);
        }
    }
}
