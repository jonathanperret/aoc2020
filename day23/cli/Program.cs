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
    public static void D(string s)
    {
        // W(s);
    }

    public static int maxVal = 9;
    public static int[] Move(int[] cups)
    {
        int currentCup = cups[0];
        int[] picked = cups[1..4];
        D($"current {currentCup} picked {picked.ToDelimitedString(",")}");
        D($"remaining {cups[4..].ToDelimitedString(",")}");
        int dest = currentCup;
        int destpos = -1;
        int t = 0;
        while (destpos < 0 && t++ < 2 * maxVal)
        {
            dest = dest == 1 ? maxVal : (char)(dest - 1);
            destpos = Array.IndexOf(cups, dest, 4) - 4;
        }
        D($"dest={dest} destpos={destpos} t={t}");
        int[] result = new int[cups.Length];
        result[^1] = currentCup;
        Array.Copy(cups, 4, result, 0, destpos + 1);
        Array.Copy(picked, 0, result, destpos + 1, 3);
        Array.Copy(cups, 4 + destpos + 1, result, destpos + 4, cups.Length - 4 - destpos - 1);
        D($"result={result.ToDelimitedString(",")}");
        return result;
    }

    public static string After1(int[] cups)
    {
        int onePos = Array.IndexOf(cups, 1);
        int[] result = new int[cups.Length - 1];
        int rightLen = cups.Length - onePos - 1;
        int leftLen = onePos;
        if (rightLen > 0)
            Array.Copy(cups, (onePos + 1) % cups.Length, result, 0, rightLen);
        if (leftLen > 0)
            Array.Copy(cups, 0, result, rightLen, leftLen);
        return result.ToDelimitedString("");
    }

    public static string Part1(string sequence)
    {
        var cups = sequence.Trim().Select(c => (int)c - '0').ToArray();
        maxVal = 9;

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
