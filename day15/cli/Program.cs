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
    public static int Solve(int[] numbers, int count = 2020)
    {
        int last = 0;
        var lastpos = new Dictionary<int, int>();
        var previouspos = new Dictionary<int, int>();
        var ages = new Dictionary<int, int>();
        for (int i = 0; i < count; i++)
        {
            if (i < numbers.Length)
            {
                last = numbers[i];
            }
            else
            {
                if (!previouspos.ContainsKey(last))
                {
                    last = 0;
                }
                else
                {
                    last = lastpos[last] - previouspos[last];
                }

            }
            if (lastpos.ContainsKey(last))
                previouspos[last] = lastpos[last];
            lastpos[last] = i;
            W(last);
        }
        return last;
    }

    static void Main(string[] args)
    {
        var text = File.ReadAllText("input.txt");
        // var text = File.ReadAllText("example.txt");
        var numbers = text.Split(",").Select(int.Parse).ToArray();
        var result = Solve(numbers, 30000000);
        W($"{result}");
    }
}