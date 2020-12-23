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
    public static string Move(string cups)
    {
        char currentCup = cups[0];
        string picked = cups[1..4];
        W($"current {currentCup} picked {picked}");
        string remaining = cups[4..];
        W($"remaining {remaining}");
        char dest = currentCup;
        int destpos = -1;
        int t = 0;
        while (destpos < 0 && t++ < 11)
        {
            dest = dest == '1' ? '9' : (char)(dest - 1);
            destpos = remaining.IndexOf(dest);
        }
        W($"dest={dest} destpos={destpos} t={t}");
        return $"{remaining[0..(destpos + 1)]}{picked}{remaining[(destpos + 1)..]}{currentCup}";
    }

    public static string After1(string cups)
    {
        // fake
        return cups[1..];
    }

    public static string Part1(string sequence)
    {
        var cups = sequence.Trim();

        return After1(MoreEnumerable.Generate(cups, Move).ElementAt(100));
    }

    static void Main(string[] args)
    {
        var text = File.ReadAllText("input.txt");
        // var numbers = lines.Select(int.Parse).ToArray();
        // var blocks = numbers.Window(26);
        // int max = numbers.Max();
        // int min = numbers.Min();

        var result = Part1(text);
        W($"{result}");
    }
}
