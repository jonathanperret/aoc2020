using System;
using System.IO;
using System.Linq;
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

    public static (int width, (long id, int orientation, string[] matrix)[] placed)? Recurse((long id, string[] matrix)[] remaining, (long id, int orientation, string[] matrix)[] placed, int width)
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
            string[][] variants = GetVariants(nextToPlace.matrix);

            // variants.ForEach(m => W(m.ToDelimitedString("\n") + "\n"));
            bool firstRow = placed.Length < width;
            bool firstInRow = placed.Length % width == 0;
            for (int orientation = 0; orientation < variants.Length; orientation++)
            {
                var variant = variants[orientation];
                W($"placing {nextToPlace.id},{orientation} in {placed.Length} placed, {nextRemaining.Length} remaining");
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
                // W($"recursing with {placed.Length} placed");

                var sol = Recurse(nextRemaining, Enumerable.Append(placed, (nextToPlace.id, orientation, variant)).ToArray(), width);
                if (sol != null)
                {
                    return sol;
                }
            }
        }

        return null;
    }

    private static string[][] GetVariants(string[] matrix)
    {
        var rotated = MoreEnumerable.Generate(matrix, Rotate).Take(4).ToArray();
        var variants = rotated.Concat(rotated.Select(Flip)).ToArray();
        return variants;
    }

    static readonly (int id, int orientation)[] knownSolution = new[] {
        (2521,0),(2239,6),(1531,0),
        (1223,7),(1511,7),(2549,1),
        (3049,5),(1847,6),(2621,2),
        (3407,3),(3881,6),(1061,0),
        (1861,0),(1481,3),(1471,2),
        (1637,0),(3727,3),(1979,3),
        (1289,0),(1217,5),(2543,5),
        (3559,6),(1249,7),(3697,1),
        (2371,3),(2267,1),(3929,5),
        (2341,6),(3947,7),(2819,0),
        (1051,2),(1361,0),(3499,5),
        (1459,1),(1999,3),(2137,1),
        (3371,2),(2551,5),(1109,6),
        (1889,2),(3847,1),(2591,1),
        (1381,3),(2089,0),(1213,5),
        (1583,5),(2683,5),(3769,5),
        (3089,2),(1423,1),(3637,5),
        (2953,0),(2699,2),(2801,2),
        (2399,6),(3169,2),(1303,7),
        (2131,3),(2053,2),(3469,5),
        (2833,2),(2663,2),(1483,7),
        (2467,7),(1319,6),(2129,6),
        (1429,5),(2503,6),(1913,0),
        (1523,1),(1427,6),(1873,2),
        (1723,6),(1789,1),(2351,7),
        (1831,6),(3359,3),(3041,3),
        (3761,6),(1019,3),(1493,6),
        (3943,2),(1663,7),(2161,6),
        (3529,0),(2861,7),(1283,1),
        (3301,2),(3989,0),(3539,6),
        (2333,0),(2207,1),(1933,2),
        (3037,1),(2213,2),(2851,5),
        (3491,2),(3581,0),(1499,3),
        (2011,2),(3331,1),(2293,1),
        (1973,1),(1931,1),(2837,3),
        (2083,2),(2377,1),(2113,7),
        (3851,6),(1867,6),(2417,5),
        (3671,2),(3467,2),(1307,2),
        (1619,1),(1543,7),(3319,7),
        (2287,2),(3191,5),(1279,2),
        (1181,3),(2711,6),(1907,5),
        (2677,6),(2927,2),(3677,1),
        (3719,5),(1607,7),(2099,2),
        (1801,5),(1439,0),(1193,2),
        (2633,1),(2917,3),(2707,7),
        (1163,7),(2281,0),(2689,6),
        (2423,2),(3373,5),(1229,3),
        (1009,5),(2797,6),(3067,6)
    };

    public static long Part1(string[] lines)
    {
        var tiles = ParseTiles(lines);
        var solution = Recurse(tiles, new (long id, int orientation, string[] matrix)[] { }, (int)Math.Sqrt(tiles.Length));
        if (solution.HasValue)
        {
            var (width, placed) = solution.Value;

            W(placed.Batch(3).Select(row => row.Select(tile => $"({tile.id},{tile.orientation})").ToDelimitedString(",")).ToDelimitedString(",\n"));

            return placed[0].id * placed[width - 1].id * placed[placed.Length - width].id * placed[placed.Length - 1].id;
        }
        return -1;
    }

    private static (long id, string[] matrix)[] ParseTiles(string[] lines)
    {
        return lines.Split("").Select(tileLines =>
        {
            long tileId = long.Parse(tileLines.ElementAt(0).Split(' ', ':')[1]);
            string[] tileMatrix = tileLines.Skip(1).ToArray();

            return (id: tileId, matrix: tileMatrix);
        }).ToArray();
    }

    private static readonly string[] monsterPattern = {
        "                  # ",
        "#    ##    ##    ###",
        " #  #  #  #  #  #   ",
    };

    public static ((int, int)[] monsters, long roughness) Part2(string[] lines, int width, (int id, int orientation)[] placement)
    {
        string[] image = MakeImage(lines, width, placement);
        int imageWidth = image[0].Length;
        int imageHeight = image.Length;

        int monsterWidth = monsterPattern[0].Length;
        int monsterHeight = monsterPattern.Length;

        var monsterREs = monsterPattern.Select(monsterRow => new Regex("^" + monsterRow.Replace(' ', '.') + "$"));

        (int x, int y)[] bestMonsters = new (int, int)[] { };
        string[] bestImage = new string[] { };
        foreach (var variant in GetVariants(image))
        {
            // W(variant.ToDelimitedString("\n") + "\n");

            var foundMonsters = new List<(int, int)>();
            for (int y = 0; y < imageHeight - (monsterHeight - 1); y++)
            {
                for (int x = 0; x < imageWidth - (monsterWidth - 1); x++)
                {
                    // W($"checking for monster at {x},{y}");
                    if (monsterREs.Select((monsterRE, i) =>
                    {
                        string substr = variant[y + i].Substring(x, monsterWidth);
                        // W($"{x},{y},{i} checking '{substr}' against '{monsterRE.ToString()}'");
                        return monsterRE.IsMatch(substr);
                    }).All(x => x))
                    {
                        W($"found monster at {x},{y}");
                        foundMonsters.Add((x, y));
                    }
                }

            }

            if (foundMonsters.Count > bestMonsters.Length)
            {
                W($"found {foundMonsters.Count} monsters: {foundMonsters.ToDelimitedString(",")}");
                bestMonsters = foundMonsters.ToArray();
                bestImage = variant;
            }
        }

        bestMonsters.ForEach((monster) =>
        {
            for (int y = monster.y; y < monster.y + monsterHeight; y++)
            {
                for (int x = monster.x; x < monster.x + monsterWidth; x++)
                {
                    if (monsterPattern[y - monster.y][x - monster.x] == '#')
                        bestImage[y] = bestImage[y].Substring(0, x) + 'O' + bestImage[y].Substring(x + 1);
                }
            }
        });

        W(bestImage.ToDelimitedString("\n") + "\n");

        int roughness = bestImage.Select(row => row.Count(c => c == '#')).Sum();

        W(roughness);

        return (bestMonsters, roughness);
    }

    public static string[] MakeImage(string[] lines, int width, (int id, int orientation)[] placement)
    {
        var tiles = ParseTiles(lines).ToDictionary(
            tile => tile.id,
            tile => tile.matrix
        );
        var orderedTiles = placement.Select(place => GetVariants(tiles[place.id])[place.orientation]);
        // orderedTiles.ForEach(matrix => W(matrix.ToDelimitedString("\n") + "\n"));
        // W(Paste(width, orderedTiles, spaced: true).ToDelimitedString("\n") + "\n");

        var strippedTiles = orderedTiles.Select(tile => tile.Slice(1, tile.Length - 2).Select(row => row.Substring(1, row.Length - 2)).ToArray());
        // strippedTiles.ForEach(matrix => W(matrix.ToDelimitedString("\n") + "\n"));

        string[] image = Paste(width, strippedTiles);
        // W(Paste(width, strippedTiles).ToDelimitedString("\n") + "\n");
        return image;
    }

    private static string[] Paste(int width, IEnumerable<string[]> tiles, bool spaced = false)
    {
        int tileWidth = tiles.ElementAt(0).Length;
        return tiles.Batch(width).SelectMany(rowTiles =>
        {
            var rows = Enumerable.Range(0, tileWidth).Select(i => rowTiles.Select(t => t[i]).ToDelimitedString(spaced ? " " : ""));
            if (!spaced)
                return rows;
            else
                return Enumerable.Append(rows, "");
        }).ToArray();
    }

    static void Main(string[] args)
    {
        var lines = File.ReadAllLines("input.txt");

        // var result = Part1(lines);
        var result = Part2(lines, 12, knownSolution);
        W($"{result.roughness}");
    }
}