using System;
using System.IO;
using System.Linq;
using Sprache;
using System.Numerics;
//using MoreLinq;
using System.Text.RegularExpressions;

namespace cli
{

    public static class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("day5.txt");

            var seats = lines.Select(line => line.Replace("F", "0").Replace("B", "1").Replace("R", "1").Replace("L", "0"))
            .Select(bin => Convert.ToInt64(bin, 2))
            .ToHashSet();

            long result = Enumerable.Range(0, 1000).Where(x => !seats.Contains(x) && seats.Contains(x - 1) && seats.Contains(x + 1)).First();

            Console.WriteLine($"{result}");
        }
    }
}
