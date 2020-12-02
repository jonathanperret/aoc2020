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
            var lines = File.ReadAllLines("day2.txt");

            int validCount = 0;
            foreach (string line in lines)
            {
                var (min, max, letter, password) = Line.Parse(line);
                int letterCount = password.Where(c => c == letter).Count();
                bool valid =
                    min <= password.Length && max <= password.Length && (
                        password.ElementAt(min - 1) == letter ^ password.ElementAt(max - 1) == letter
                    );
                if (valid) validCount++;
                Console.WriteLine($"{min} {max} {letter} {password} {letterCount} {valid}");
            }

            Console.WriteLine(validCount);
        }
    }
}
