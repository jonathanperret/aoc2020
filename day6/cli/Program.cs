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
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("day6.txt");

            var groups = lines.Split("")
            .Cast<IEnumerable<IEnumerable<char>>>()
            .Debug(s => s.ToDelimitedString(" "))
            ;

            var questions = groups
            .Select(g => g.Aggregate((s1, s2) => s1.Intersect(s2)))
            .Debug(s => s.ToDelimitedString(":"))
            .Select(g => g.Count()).Sum();

            Console.WriteLine(questions);
        }
    }
}
