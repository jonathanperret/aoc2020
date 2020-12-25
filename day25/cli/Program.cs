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
    public static long Part1(long cardKey, long doorKey)
    {
        int cardLoopSize = FindLoopSize(cardKey);
        int doorLoopSize = FindLoopSize(doorKey);
        W($"card {cardLoopSize}, door {doorLoopSize}");

        long subject = doorKey;
        long value = 1;
        for (int i = 1; i <= cardLoopSize; i++)
        {
            // W($"{i}: {value}");
            value *= subject;
            value = value % 20201227;
        }

        return value;
    }

    private static int FindLoopSize(long cardKey)
    {
        long subject = 7;
        long value = 1;
        for (int cardLoopSize = 1; cardLoopSize <= 100000000; cardLoopSize++)
        {
            // W($"{cardLoopSize}: {value}");
            value *= subject;
            value = value % 20201227;
            if (value == cardKey) return cardLoopSize;
        }
        throw new Exception("can't find loop size");
    }

    static void Main(string[] args)
    {
        var lines = File.ReadAllLines("input.txt").Select(long.Parse).ToArray();

        var result = Part1(lines[0], lines[1]);
        W($"{result}");
    }
}
