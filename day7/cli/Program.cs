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

            // while (true)
            // {
            //     int current = goodtypes.Length;
            //     var newtypes = lines.Where(line => goodtypes.Any(t => line.Contains(" " + t + " ")))
            //     .Select(line => line.Split(" bags contain ")[0]).ToArray();
            //     W(newtypes.Length);
            //     goodtypes = goodtypes.Concat(newtypes).Distinct().ToArray();
            //     W(goodtypes.Length);
            //     if (goodtypes.Length <= current) break;
            // }

            // W(goodtypes.OrderBy(x => x).Str("\n"));

            // W(goodtypes.Length - 1);


            var tofill = new List<(int count, string type)>() { (1, mybag) };
            int tobuy = 0;

            while (tofill.Count > 0)
            {
                int current = tobuy;
                foreach (string line in lines)
                {
                    var (type, contents) = line.Split(" bags contain ");
                    contents = contents.Replace(" bag, ", ",").Replace(" bags, ", ",").Replace(" bags.", "").Replace(" bag.", "");
                    if (tofill.Any(pair => pair.type == type))
                    {
                        W($"filling {type}");
                        var (count, _) = tofill.Find(pair => pair.type == type);
                        tofill.Remove((count, type));
                        W($"buying {count} of {type}");
                        tobuy += count;
                        foreach (string contained in contents.Split(","))
                        {
                            if (contained == "no other") continue;
                            W($"parsing {contained}");
                            var words = contained.Split(" ");
                            var (containedCount, containedType) = (int.Parse(words[0]), words[1] + " " + words[2]);
                            W($"adding {containedCount}*{count} of {containedType}");
                            tofill.Add((containedCount * count, containedType));
                        }
                    }
                }
            }
            W($"bought {tobuy - 1} bags");

        }
    }
}
