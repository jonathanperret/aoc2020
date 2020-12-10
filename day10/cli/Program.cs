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
            var levels = lines.Select(int.Parse).OrderBy(x => x).ToArray();
            var blocks = levels.Window(26);
            int maxlevel = levels.Max();
            int devicelevel = maxlevel + 3;

            var waysToReach = new long[devicelevel];
            waysToReach[0] = 1;
            foreach (int level in levels)
            {
                if (level >= 1) waysToReach[level] += waysToReach[level - 1];
                if (level >= 2) waysToReach[level] += waysToReach[level - 2];
                if (level >= 3) waysToReach[level] += waysToReach[level - 3];
                W($"{level} : {waysToReach[level]}");
            }

            long result = waysToReach[devicelevel - 1]
                + waysToReach[devicelevel - 2]
                + waysToReach[devicelevel - 3];
            W($"{devicelevel}: {result}");
        }
    }
}
