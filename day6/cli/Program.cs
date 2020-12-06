using System;
using System.IO;
using System.Linq;
using Sprache;
using System.Numerics;
using MoreLinq;
using System.Text.RegularExpressions;

namespace cli
{

    public static class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("day6.txt");
            var groups = lines.Split("");

            var questions = groups.Select(g => g.Aggregate((s1, s2) => s1.Distinct().Intersect(s2.Distinct()).ToDelimitedString("")).Count()).Sum();
            Console.WriteLine(questions);
        }
    }
}
