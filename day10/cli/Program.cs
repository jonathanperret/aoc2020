using System;
using System.IO;
using System.Linq;
using static cli.Util;

namespace cli
{
    public static class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");
            var levels = lines.Select(int.Parse).ToArray();

            int devicelevel = levels.Max() + 3;

            var knownLevels = levels.Append(devicelevel).ToHashSet();

            var waysToReach = Memoize<int, long>((level, recur) =>
            {
                if (level == 0) return 1;
                if (!knownLevels.Contains(level)) return 0;

                return recur(level - 1) + recur(level - 2) + recur(level - 3);
            });

            long result = waysToReach(devicelevel);
            W($"{devicelevel}: {result}");
        }
    }
}
