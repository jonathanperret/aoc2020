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
    static void Main(string[] args)
    {
        var text = File.ReadAllText("input.txt");
        var lines = File.ReadAllLines("input.txt");

        var matrix = lines.Select(
            line => line.ToCharArray()
        ).ToArray();


        int result = 0;
        int iter = 0;
        for (iter = 0; iter < 100000; iter++)
        {
            matrix = matrix.Select((row, rowIndex) =>
            {
                return row.Select((seat, colIndex) =>
                {
                    int scan(int dr, int dc)
                    {
                        int r = rowIndex + dr, c = colIndex + dc;
                        while (r >= 0 && r < matrix.Length && c >= 0 && c < row.Length && matrix[r][c] == '.')
                        {
                            r += dr; c += dc;
                        }
                        return (r >= 0 && r < matrix.Length && c >= 0 && c < row.Length && matrix[r][c] == '#') ? 1 : 0;
                    }
                    int neighbours =
                        scan(-1, -1) +
                        scan(-1, 0) +
                        scan(-1, 1) +
                        scan(0, -1) +
                        scan(0, 1) +
                        scan(1, -1) +
                        scan(1, 0) +
                        scan(1, 1);
                    if (seat == 'L' && neighbours == 0)
                    {
                        return '#';
                    }
                    else if (seat == '#' && neighbours >= 5)
                    {
                        return 'L';
                    }
                    else
                    {
                        return seat;
                    }

                }).ToArray();
            }).ToArray();

            int oldResult = result;
            result = matrix.Select(row => row.Count(c => c == '#')).Sum();
            if (oldResult == result) break;
        }

        W($"{result} after {iter}");
    }
}