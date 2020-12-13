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
        var lines = File.ReadAllLines("input.txt").ToArray();
        int timestamp = int.Parse(lines[0]);
        var buses = lines[1].Split(",").Where(s => s != "x").Select(int.Parse);

        buses.Select(b =>
        {
            int wait = b - (timestamp % b);
            int result = wait * b;
            W($"{b} {wait} {result}");
            return result;
        }).Consume();

        int result = buses.Count();
        W($"{result}");
    }
}