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

            blocks.Select(block =>
            {
                var preamble = block.Take(25);
                var last = block.Skip(25).First();

                var matches = preamble.Subsets(2).Where(subset => subset.ElementAt(0) != subset.ElementAt(1) && subset.Sum() == last);
                //W(matches.Select(m => m.Fold((a, b) => $"{a} + {b} = {a + b} == {last}")).ToDelimitedString(", "));
                if (!matches.Any())
                {
                    W(last);
                }

                return true;
            }).Count();
        }
    }
}
