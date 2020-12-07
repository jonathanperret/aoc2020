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
            var lines = File.ReadAllLines("day7.txt");
            var text = File.ReadAllText("day7.txt");
            var groups = lines.Split("");

            string mybag = "shiny gold";

            string[] goodtypes = { mybag };

            while (true)
            {
                int current = goodtypes.Length;
                var newtypes = lines.Where(line => goodtypes.Any(t => line.Contains(" " + t + " ")))
                .Select(line => line.Split(" bags contain ")[0]).ToArray();
                W(newtypes.Length);
                goodtypes = goodtypes.Concat(newtypes).Distinct().ToArray();
                W(goodtypes.Length);
                if (goodtypes.Length <= current) break;
            }

            W(goodtypes.OrderBy(x => x).Str("\n"));

            W(goodtypes.Length - 1);


            // lines.Select(line =>
            // {
            //     var (type, contents) = line.Split(" bags contain ");
            //     contents = contents.Replace(" bag, ", ", ").Replace(" bags, ", ", ").Replace(" bags.", "").Replace(" bag.", ""); ;
            //     W(contents);
            //     return type;
            // }).Count();
        }
    }
}
