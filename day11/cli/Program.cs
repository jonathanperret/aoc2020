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
        var lines = File.ReadAllLines("input.txt");

        var matrix = lines.Select(
            line => line.ToCharArray()
        ).ToArray();

        int iter = 0;
        for (iter = 0; iter < 1000; iter++)
        {
            var newMatrix = matrix.AsParallel().Select((row, rowIndex) =>
            {
                return row.Select((seat, colIndex) =>
                {
                    bool inPlane(int r, int c)
                    {
                        return r >= 0 && r < matrix.Length && c >= 0 && c < row.Length;
                    }
                    int scan(int dr, int dc)
                    {
                        int r = rowIndex, c = colIndex;
                        do
                        {
                            r += dr; c += dc;
                            if (!inPlane(r, c)) return 0;
                        }
                        while (matrix[r][c] == '.');
                        return matrix[r][c] == '#' ? 1 : 0;
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

            // W("---");
            // W(matrix.Select(row => row.ToDelimitedString("")).ToDelimitedString("\n"));

            bool same = newMatrix.SelectMany(row => row).SequenceEqual(matrix.SelectMany(row => row));
            if (same) break;

            matrix = newMatrix;
        }
        int occupied = matrix.SelectMany(row => row).Count(c => c == '#');

        W($"{occupied} after {iter}");
    }
}