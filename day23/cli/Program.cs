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

    public static int Move(int[] next, int current)
    {
        int maxVal = next.Length - 1;
        int picked1 = next[current];
        int picked2 = next[picked1];
        int picked3 = next[picked2];
        // remove picked from circle
        next[current] = next[picked3];
        // pick destination
        int dest = current;
        while (true)
        {
            dest = dest > 1 ? dest - 1 : maxVal;
            if (dest != picked1 && dest != picked2 && dest != picked3)
                break;
        }
        // insert picked right of dest
        next[picked3] = next[dest];
        next[dest] = picked1;

        return next[current];
    }

    public static string After1(int[] next)
    {
        var result = new List<int>();
        int current = 1;
        for (int i = 0; i < next.Length - 2; i++)
        {
            current = next[current];
            result.Add(current);
        }
        return result.ToDelimitedString("");
    }

    public static int[] BuildNext(int[] cups)
    {
        int[] result = new int[cups.Length + 1];
        for (int i = 0; i < cups.Length; i++)
        {
            int cup = cups[i];
            int nextCup = i < cups.Length - 1 ? cups[i + 1] : cups[0];
            result[cup] = nextCup;
        }
        return result;
    }

    public static string Part1(string sequence)
    {
        var cups = sequence.Trim().Select(c => (int)c - '0').ToArray();
        var next = BuildNext(cups);
        int current = cups[0];
        for (int i = 0; i < 100; i++)
        {
            current = Move(next, current);
        }

        return After1(next);
    }

    public static string Part2(string sequence)
    {
        int maxVal = 1000000;
        var cups = sequence.Trim().Select(c => (int)c - '0')
            .Concat(Enumerable.Range(10, maxVal - 9))
            .ToArray();

        W($"max={cups.Max()} count={cups.Length}");

        return "oioi";
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
