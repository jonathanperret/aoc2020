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

            // var (min, max) = File.ReadAllText("day4.txt").Split("-").Select(int.Parse);
            // int result = MoreEnumerable.Sequence(min, max).Select(x=>x.ToString()).Where(x =>
            // {
            //     string s = x.ToString();

            //     return
            //         s.Pairwise((c1, c2) => c2 >= c1).All(x => x) &&
            //         s.GroupAdjacent(c => c).Count(g => g.Count() == 2) >= 1;
            // }).Count();

            int result = 0;

            Console.WriteLine($"{result}");
        }
    }
}
