using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using MoreLinq;
using Sprache;

namespace cli
{
    public static class Program
    {
        public static long Product(this IEnumerable<long> seq)
        {
            return seq.Aggregate((a, b) => a * b);
        }

        public static IEnumerable<IEnumerable<TResult>> Matrix<TRow, TColumn, TResult>(this IEnumerable<TRow> rows, IEnumerable<TColumn> columns, Func<TRow, TColumn, TResult> fn)
        {
            return rows.Select((row) => columns.Select(column => fn(row, column)));
        }

        public static IEnumerable<IEnumerable<TResult>> Matrix<TRow, TColumn, TResult>(this IEnumerable<TRow> rows, IEnumerable<TColumn> columns, Func<TRow, TColumn, int, int, TResult> fn)
        {
            return rows.Select((row, rowIndex) => columns.Select((column, columnIndex) => fn(row, column, rowIndex, columnIndex)));
        }

        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("day3.txt");

            (int x, int y)[] slopes = { (1, 1), (3, 1), (5, 1), (7, 1), (1, 2) };

            long total =
                        lines.Matrix(slopes,
                              (line, s, y, x) => y % s.y == 0 && line[s.x * y / s.y % line.Length] == '#' ? 1L : 0
                        )
                            .Transpose()
                            .Select(s => s.Sum())
                            .Product();

            Console.WriteLine(total);
        }
    }
}
