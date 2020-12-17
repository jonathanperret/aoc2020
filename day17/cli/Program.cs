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
        var matrix = new bool[][][][] { new bool[][][] { lines.Select(l => l.Select(c => c == '#').ToArray()).ToArray() } };
        bool[][][][] step(bool[][][][] matrix)
        {
            var W = matrix[0][0][0].Length;
            var H = matrix[0][0].Length;
            var D = matrix[0].Length;
            var T = matrix.Length;

            bool isactive(int x, int y, int z, int t)
            {
                return x >= 0 && x < W &&
                    y >= 0 && y < H &&
                    z >= 0 && z < D &&
                    t >= 0 && t < T &&
                    matrix[t][z][y][x];
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
                                total += isactive(xx, yy, zz, tt) ? 1 : 0;
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
                                if (isactive(x, y, z, t))
                                {
                                    return (n == 2 || n == 3);
                                }
                                else
                                {
                                    return n == 3;
                                }
                            }
                        ).ToArray()
                    ).ToArray()
                ).ToArray()
            ).ToArray();

            return newmatrix;
        }

        bool[][][][] newmatrix = matrix;
        for (int i = 0; i < 6; i++)
        {
            var st = System.Diagnostics.Stopwatch.StartNew();
            newmatrix = step(newmatrix);
            //Console.WriteLine(newmatrix.Select(dim => dim.Select(plane => plane.Select(row => row.Select(b => b ? '#' : '.').ToDelimitedString("")).ToDelimitedString("\n")).ToDelimitedString("\n===\n")).ToDelimitedString("\n\n\n"));
            Console.WriteLine($"--- {i} {System.GC.GetTotalMemory(true):0.#e+0} bytes {st.ElapsedMilliseconds}ms");
        }

        return newmatrix.Sum(dim => dim.Sum(plane => plane.Sum(row => row.Count(c => c))));
    }

    static void Main(string[] args)
    {
        var lines = File.ReadAllLines("input.txt");

        var result = Solve(lines);
        W($"{result}");
    }
}