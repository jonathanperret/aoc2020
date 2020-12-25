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
    const long modulus = 20201227;
    public static long Part1(long cardKey, long doorKey)
    {
        int cardLoopSize = FindLoopSize(cardKey);
        W($"card {cardLoopSize}");

        return (long)BigInteger.ModPow(doorKey, cardLoopSize, modulus);
    }

    private static int FindLoopSize(long targetKey)
    {
        long subject = 7;
        var seq = MoreEnumerable.Generate((value: 1L, index: 0), (t) => (t.value * subject % modulus, t.index + 1));
        return seq.First(t => t.value == targetKey).index;
    }

    static void Main(string[] args)
    {
        var lines = File.ReadAllLines("input.txt").Select(long.Parse).ToArray();

        var result = Part1(lines[0], lines[1]);
        W($"{result}");
    }
}
