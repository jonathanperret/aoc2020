using System;
using System.IO;
using System.Linq;
using Sprache;
using System.Numerics;
using MoreLinq;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using static Util;

public static class Program
{
    public static (int, int)[] Part1(string[] lines)
    {
        var flipped = lines.Select(line =>
        {
            string[] dirs = Regex.Matches(line, "e|w|ne|nw|se|sw").Select(m => m.Value).ToArray();

            (int, int) coord =
                dirs.Aggregate((0, 0), (c, dir) =>
                {
                    var (x, y) = c;
                    return dir switch
                    {
                        "e" => (x + 1, y),
                        "w" => (x - 1, y),
                        "ne" => (x, y + 1),
                        "nw" => (x - 1, y + 1),
                        "sw" => (x, y - 1),
                        "se" => (x + 1, y - 1),
                        _ => throw new Exception("bad dir")
                    };
                });

            // W($"{dirs.Str(",")} => {coord}");
            return coord;
        });

        // W(flipped.OrderBy(c => c).Str(","));

        var black = flipped.GroupBy(c => c).Select(g => g.ToArray()).Where(g => g.Length % 2 == 1).Select(g => g[0]);

        return black.ToArray();
    }

    public static int Part2((int x, int y)[] black, int days)
    {
        static IEnumerable<int> neighbors(int c)
        {
            yield return c + 1;
            yield return c - 1;
            yield return c + 1000;
            yield return c + 999;
            yield return c - 1000;
            yield return c - 999;
        }

        var blackInts = black.Select(c => (x: c.x + 500, y: c.y + 500)).Assert(c => c.x > 0 && c.x < 1000 && c.y > 0 && c.y < 1000)
            .Select(c => c.y * 1000 + c.x);

        var blackSet = Enumerable.ToHashSet(blackInts);

        for (int i = 0; i < days; i++)
        {
            var neighborList = blackSet.SelectMany(neighbors);
            var newBlack = neighborList
                .CountBy(c => c)
                .Choose(kv =>
                    (kv.Value == 2 || (kv.Value == 1 && blackSet.Contains(kv.Key)), kv.Key)
                );
            blackSet = Enumerable.ToHashSet(newBlack);

            // W($"Day {i + 1}: {blackSet.Count}");
        }
        return blackSet.Count;
    }

    static void Main(string[] args)
    {
        var text = File.ReadAllText("input.txt");
        var lines = File.ReadAllLines("input.txt");
        // var numbers = lines.Select(int.Parse).ToArray();
        // var blocks = numbers.Window(26);
        // int max = numbers.Max();
        // int min = numbers.Min();

        var black = Part1(lines);
        W($"{black.Length}");
        var result = Part2(black, 100);
        W($"{result}");
    }
}
