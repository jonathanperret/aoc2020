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
        var groups = lines.Split("");
        var numbers = lines.Select(int.Parse).ToArray();
        var blocks = numbers.Window(26);
        int max = numbers.Max();
        int min = numbers.Min();

        int result = max - min;
        W($"{result}");
    }
}