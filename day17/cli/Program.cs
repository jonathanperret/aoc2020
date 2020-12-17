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
    public static int Solve(string[] lines, int iterations = 6)
    {
        var matrix = lines.SelectMany(l => l.Select(c => c == '#')).ToArray();

        static int index(int x, int y, int z, int t, int W, int H, int D, int T)
        {
            return x + W * (y + H * (z + D * t));
        }

        bool[] step(bool[] matrix, int W, int H, int D, int T)
        {
            bool isactive(int x, int y, int z, int t)
            {
                return
                    x >= 0 && x < W &&
                    y >= 0 && y < H &&
                    z >= 0 && z < D &&
                    t >= 0 && t < T &&
                    matrix[index(x, y, z, t, W, H, D, T)];
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

            var newmatrix = new bool[(T + 2) * (D + 2) * (H + 2) * (W + 2)];
            int i = 0;
            for (int t = -1; t <= T; t++)
            {
                for (int z = -1; z <= D; z++)
                {
                    for (int y = -1; y <= H; y++)
                    {
                        for (int x = -1; x <= W; x++)
                        {
                            int n = neighbors(x, y, z, t);
                            if (isactive(x, y, z, t))
                            {
                                newmatrix[i] = (n == 2 || n == 3);
                            }
                            else
                            {
                                newmatrix[i] = n == 3;
                            }
                            i++;
                        }
                    }
                }
            }

            return newmatrix;
        }

#pragma warning disable 8321
        static void dump(bool[] matrix, int W, int H, int D, int T)
        {
            Console.WriteLine(Enumerable.Range(0, T).Select(t =>
                   Enumerable.Range(0, D).Select(z =>
                      Enumerable.Range(0, H).Select(y =>
                         Enumerable.Range(0, W).Select(x =>
                                matrix[x + W * (y + H * (z + D * t))] ? '#' : '.'
                         ).ToDelimitedString("")
                      ).ToDelimitedString("\n")
                   ).ToDelimitedString("\n===\n")
                ).ToDelimitedString("\n\n"));
        }

        bool[] newmatrix = matrix;
        for (int i = 0; i < iterations; i++)
        {
            var st = System.Diagnostics.Stopwatch.StartNew();
            var (W, H, D, T) = (lines[0].Length + 2 * i, lines.Length + 2 * i, 1 + 2 * i, 1 + 2 * i);
            // dump(newmatrix, W, H, D, T);
            newmatrix = step(newmatrix, W, H, D, T);
            Console.WriteLine($"--- {i} {System.GC.GetTotalMemory(true):0.#e+0} bytes {st.ElapsedMilliseconds}ms");
        }

        return newmatrix.Count(c => c);
    }

    static void Main(string[] args)
    {
        var lines = File.ReadAllLines("input.txt");

        var result = Solve(lines, 60);
        W($"{result}");
    }
}