using System;
using System.IO;
using System.Linq;
using Sprache;
using System.Numerics;
using MoreLinq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace cli
{
    public static class Program
    {
        public static void W<T>(T data)
        {
            Console.WriteLine(data);
        }
        static void Main(string[] args)
        {
            var text = File.ReadAllText("input.txt");
            var lines = File.ReadAllLines("input.txt");
            var groups = lines.Split("");
            var numbers = lines.Select(long.Parse);

            var blocks = numbers.Window(26);

            long badnumber = blocks.AsParallel()
            .Choose(block =>
            {
                var preamble = block.Take(25);
                var last = block.ElementAt(25);

                var matches = preamble.Subsets(2).Where(pair => pair.Fold((a, b) => a != b && a + b == last));

                return (!matches.Any(), last);
            }).First();

            W($"{badnumber} is bad");

            var subsequences = Enumerable.Range(2, numbers.Count() - 1)
            .SelectMany(size => numbers.Window(size));

            subsequences.AsParallel()
            .Where(block => block.Sum() == badnumber)
            .Select(block =>
                $"{block.OrderBy(x => x).ToDelimitedString(" + ")} = {block.Sum()} == {badnumber}\n"
                + $"{block.Min()} + {block.Max()} = {block.Min() + block.Max()}"
            )
            .ForEach(W);
        }
    }
}
