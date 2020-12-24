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
        static (int x, int y)[] neighbors((int, int) c)
        {
            var (x, y) = c;
            return new (int, int)[]{
                (x + 1, y),
                (x - 1, y),
                (x, y + 1),
                (x - 1, y + 1),
                (x, y - 1),
                (x + 1, y - 1),
            };
        }

        var blackSet = Enumerable.ToHashSet(black);

        for (int i = 0; i < days; i++)
        {
            var white = Enumerable.ToHashSet(blackSet.SelectMany(neighbors));
            white.ExceptWith(blackSet);
            white.RemoveWhere(c => neighbors(c).Count(nb => blackSet.Contains(nb)) != 2);
            var deadblack = blackSet.Where(c =>
            {
                int n = neighbors(c).Count(nb => blackSet.Contains(nb));
                return n < 1 || n > 2;
            }).ToArray();
            blackSet.ExceptWith(deadblack);

            blackSet.UnionWith(white);

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
