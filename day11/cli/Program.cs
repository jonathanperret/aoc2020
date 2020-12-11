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
                    int neighbours =
                        (rowIndex > 0 && colIndex > 0 && matrix[rowIndex - 1][colIndex - 1] == '#' ? 1 : 0) +
                        (rowIndex > 0 && matrix[rowIndex - 1][colIndex] == '#' ? 1 : 0) +
                        (rowIndex > 0 && colIndex < row.Length - 1 && matrix[rowIndex - 1][colIndex + 1] == '#' ? 1 : 0) +
                        (colIndex > 0 && row[colIndex - 1] == '#' ? 1 : 0) +
                        (colIndex < row.Length - 1 && row[colIndex + 1] == '#' ? 1 : 0) +
                        (rowIndex < matrix.Length - 1 && colIndex > 0 && matrix[rowIndex + 1][colIndex - 1] == '#' ? 1 : 0) +
                        (rowIndex < matrix.Length - 1 && matrix[rowIndex + 1][colIndex] == '#' ? 1 : 0) +
                        (rowIndex < matrix.Length - 1 && colIndex < row.Length - 1 && matrix[rowIndex + 1][colIndex + 1] == '#' ? 1 : 0);
                    if (seat == 'L' && neighbours == 0)
                    {
                        return '#';
                    }
                    else if (seat == '#' && neighbours >= 4)
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