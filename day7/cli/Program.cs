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

            var bagTable = lines
            .Select(line => line.Split(" bags contain "))
            .ToDictionary((parts) => parts[0], (parts) =>
            {
                return parts[1].Replace(" bag, ", ",").Replace(" bags, ", ",").Replace(" bags.", "").Replace(" bag.", "")
                .Split(",")
                .Where(contained => contained != "no other")
                .Select(contained =>
                {
                    var words = contained.Split(" ", 2);
                    return (count: int.Parse(words[0]), type: words[1]);
                });
            });

            int fill(string type)
            {
                W($"buying and filling {type}");
                return 1 + bagTable[type].Select(c => c.count * fill(c.type)).Sum();
            }

            string mybag = "shiny gold";
            int tobuy = fill(mybag) - 1;
            W(tobuy);
        }
    }
}
