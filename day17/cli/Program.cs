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
    public static int Solve(params string[] lines)
    {
        var matrix = new char[][][][] { new char[][][] { lines.Select(l => l.ToCharArray()).ToArray() } };
        char[][][][] step(char[][][][] matrix)
        {
            var W = matrix[0][0][0].Length;
            var H = matrix[0][0].Length;
            var D = matrix[0].Length;
            var T = matrix.Length;

            int isactive(int x, int y, int z, int t)
            {
                return (x >= 0 && x < W &&
                    y >= 0 && y < H &&
                    z >= 0 && z < D &&
                    t >= 0 && t < T &&
                    matrix[t][z][y][x] == '#') ? 1 : 0;
            }

            int neighbors(int x, int y, int z, int t)
            {
                int total = 0;
                for (int xx = x - 1; xx <= x + 1; xx++)
                {
                    for (int yy = y - 1; yy <= y + 1; yy++)
                    {
                        for (int zz = z - 1; zz <= z + 1; zz++)
                        {
                            for (int tt = t - 1; tt <= t + 1; tt++)
                            {
                                if (xx == x && yy == y && zz == z && tt == t)
                                    continue;
                                total += isactive(xx, yy, zz, tt);
                            }
                        }
                    }
                }
                return total;
            }

            var newmatrix = Enumerable.Range(-1, T + 2).Select(
                t => Enumerable.Range(-1, D + 2).Select(
                    z => Enumerable.Range(-1, H + 2).Select(
                        y => Enumerable.Range(-1, W + 2).Select(
                            x =>
                            {
                                int n = neighbors(x, y, z, t);
                                int c = isactive(x, y, z, t);
                                if (c == 1)
                                {
                                    if (n == 2 || n == 3) return '#'; else return '.';
                                }
                                else
                                {
                                    if (n == 3) return '#'; else return '.';
                                }
                            }
                        ).ToArray()
                    ).ToArray()
                ).ToArray()
            ).ToArray();

            return newmatrix;
        }

        char[][][][] newmatrix = matrix;
        for (int i = 0; i < 6; i++)
        {
            newmatrix = step(newmatrix);
            Console.WriteLine(newmatrix.Select(dim => dim.Select(plane => plane.Select(row => row.ToDelimitedString("")).ToDelimitedString("\n")).ToDelimitedString("\n===\n")).ToDelimitedString("\n\n\n"));
            Console.WriteLine("---");
        }

        return newmatrix.Select(dim => dim.Select(plane => plane.Select(row => row.Count(c => c == '#')).Sum()).Sum()).Sum();
    }

    static void Main(string[] args)
    {
        var lines = File.ReadAllLines("input.txt");

        var result = Solve(lines);
        W($"{result}");
    }
}