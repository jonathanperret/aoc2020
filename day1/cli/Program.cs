using System;
using System.IO;
using System.Linq;
using MoreLinq;

namespace cli
{
    public static class Program
    {
        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("day1.txt");
            int[] amounts = lines.Select(l => int.Parse(l)).ToArray();

            var results = amounts.Subsets(3)
                .Where(s => s.Sum() == 2020)
                .Select(s => s.Aggregate((x, y) => x * y));

            Console.WriteLine(results.First());
        }
    }
}
