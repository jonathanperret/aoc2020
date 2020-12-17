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
        var matrix = new char[][][] { lines.Select(l => l.ToCharArray()).ToArray() };
        char[][][] step(char[][][] matrix)
        {
            var W = matrix[0][0].Length;
            var H = matrix[0].Length;
            var D = matrix.Length;

            int isactive(int x, int y, int z)
            {
                return (x >= 0 && x < W &&
                    y >= 0 && y < H &&
                    z >= 0 && z < D &&
                    matrix[z][y][x] == '#') ? 1 : 0;
            }

            int neighbors(int x, int y, int z)
            {
                int total = 0;
                for (int xx = x - 1; xx <= x + 1; xx++)
                {
                    for (int yy = y - 1; yy <= y + 1; yy++)
                    {
                        for (int zz = z - 1; zz <= z + 1; zz++)
                        {
                            if (xx == x && yy == y && zz == z)
                                continue;
                            total += isactive(xx, yy, zz);
                        }
                    }
                }
                return total;
            }

            var newmatrix = Enumerable.Range(-1, D + 2).Select(
                z => Enumerable.Range(-1, H + 2).Select(
                    y => Enumerable.Range(-1, W + 2).Select(
                        x =>
                        {
                            int n = neighbors(x, y, z);
                            int c = isactive(x, y, z);
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
            ).ToArray();

            return newmatrix;
        }

        char[][][] newmatrix = matrix;
        for (int i = 0; i < 6; i++)
        {
            newmatrix = step(newmatrix);
            Console.WriteLine(newmatrix.Select(plane => plane.Select(row => row.ToDelimitedString("")).ToDelimitedString("\n")).ToDelimitedString("\n\n"));
            Console.WriteLine("---");
        }

        return newmatrix.Select(plane => plane.Select(row => row.Count(c => c == '#')).Sum()).Sum();
    }

    static void Main(string[] args)
    {
        var lines = File.ReadAllLines("input.txt");

        var result = Solve(lines);
        W($"{result}");
    }
}