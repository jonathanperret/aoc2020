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
    public static string[] Rotate(string[] matrix)
    {
        return matrix.Transpose().Select(a => a.Reverse().ToDelimitedString("")).ToArray();
    }

    public static string[] Flip(string[] matrix)
    {
        return matrix.Select(s => s.Reverse().ToDelimitedString("")).ToArray();
    }

    public static bool MatchRight(string[] left, string[] right)
    {
        return left.Select(a => a.Last()).SequenceEqual(
           right.Select(a => a.First())
        );
    }

    public static bool MatchBottom(string[] top, string[] bottom)
    {
        return top.Last() == bottom.First();
    }

#nullable enable

    public static (int width, (long id, string[] matrix)[] placed)? Recurse((long id, string[] matrix)[] remaining, (long id, string[] matrix)[] placed, int width)
    {
        if (remaining.Length == 0)
        {
            W($"found w={width}");
            return (width, placed);
        }

        for (int picked = 0; picked < remaining.Length; picked++)
        {
            var nextToPlace = remaining[picked];
            var nextRemaining = remaining.Take(picked).Concat(remaining.Skip(picked + 1)).ToArray();

            var variants = MoreEnumerable.Generate(nextToPlace.matrix, Rotate).Take(4);
            variants = variants.Concat(variants.Select(Flip));

            // variants.ForEach(m => W(m.ToDelimitedString("\n") + "\n"));
            bool firstRow = placed.Length < width;
            bool firstInRow = placed.Length % width == 0;
            foreach (var variant in variants)
            {
                W($"placing {nextToPlace.id} in {placed.Length} placed, {nextRemaining.Length} remaining");
                bool canPlace =
                    firstRow ?
                        (firstInRow || MatchRight(placed[placed.Length - 1].matrix, variant)) :
                        (firstInRow ?
                            MatchBottom(placed[placed.Length - width].matrix, variant) :
                            (MatchBottom(placed[placed.Length - width].matrix, variant) && MatchRight(placed[placed.Length - 1].matrix, variant)));
                if (!canPlace)
                {
                    W($"backtracking from {placed.Length} placed");
                    continue;
                }
                W($"recursing with {placed.Length} placed");

                var sol = Recurse(nextRemaining, Enumerable.Append(placed, (nextToPlace.id, variant)).ToArray(), width);
                if (sol != null)
                {
                    return sol;
                }
            }
        }

        return null;
    }

    public static long Solve(string[] lines)
    {
        var tiles = lines.Split("").Select(tileLines =>
        {
            long tileId = long.Parse(tileLines.ElementAt(0).Split(' ', ':')[1]);
            string[] tileMatrix = tileLines.Skip(1).ToArray();

            return (id: tileId, matrix: tileMatrix);
        }).ToArray();
        var solution = Recurse(tiles, new (long id, string[] matrix)[] { }, (int)Math.Sqrt(tiles.Length));
        if (solution.HasValue)
        {
            var (width, placed) = solution.Value;

            W(placed.Select(t => t.id).Batch(3).Select(row => row.ToDelimitedString(" ")).ToDelimitedString("\n"));

            return placed[0].id * placed[width - 1].id * placed[placed.Length - width].id * placed[placed.Length - 1].id;
        }
        return -1;
        //        return tiles.Select((t) => t.id).Product();
    }

    static void Main(string[] args)
    {
        var text = File.ReadAllText("input.txt");
        var lines = File.ReadAllLines("input.txt");
        // var numbers = lines.Select(int.Parse).ToArray();
        // var blocks = numbers.Window(26);
        // int max = numbers.Max();
        // int min = numbers.Min();

        var result = Solve(lines);
        W($"{result}");
    }
}