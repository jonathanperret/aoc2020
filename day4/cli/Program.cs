using System;
using System.IO;
using System.Linq;
using Sprache;
using System.Numerics;
using MoreLinq;

namespace cli
{

    public static class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("day4.txt");

            string[] required = {
                "byr",
                "iyr",
                "eyr",
                "hgt",
                "hcl",
                "ecl",
                "pid",
             };

            int result = lines.Segment(line => string.IsNullOrEmpty(line)).Select(ls => ls.ToDelimitedString(" "))
            .Select(p => p.Trim().Split(" ").Select(kv => kv.Split(":")[0]).Intersect(required))
            .Where(fields => fields.Count() == required.Count())
            .Count();
            // .ForEach(v => Console.WriteLine(v.ToDelimitedString(",")));

            // var (min, max) = File.ReadAllText("day4.txt").Split("-").Select(int.Parse);
            // int result = MoreEnumerable.Sequence(min, max).Select(x => x.ToString()).Where(s =>
            //           s.Pairwise((c1, c2) => c2 >= c1).All(x => x) &&
            //           s.GroupAdjacent(c => c).Count(g => g.Count() == 2) >= 1
            // ).Count();


            Console.WriteLine($"{result}");
        }
    }
}
