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
        public static Parser<(int, int, char, string)> Line =
            (from min in Parse.Number.Select(int.Parse)
             from dash in Parse.Char('-')
             from max in Parse.Number.Select(int.Parse).Token()
             from letter in Parse.Letter.Token()
             from colon in Parse.Char(':')
             from password in Parse.Letter.Many().Text().Token()
             select (min, max, letter, password)).End();

        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("day3.txt");
            bool[][] trees = lines.Select(line => line.Select(c => c == '#').ToArray()).ToArray();
            int width = trees[0].Length;
            int height = trees.Length;

            long total = 1;
            foreach (var (sx, sy) in new (int, int)[] { (1, 1), (3, 1), (5, 1), (7, 1), (1, 2) })
            {
                int col = 0;
                long encountered = 0;
                for (int r = 0; r < height; r += sy)
                {
                    if (trees[r][col])
                        encountered++;

                    col = (col + sx) % width;
                }
                Console.WriteLine($"{encountered}");
                total *= encountered;
            }

            Console.WriteLine($"{total}");
        }
    }
}
