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
        var lastpos = new int[count];
        int next = 0;
        for (int i = 0; i < count; i++)
        {
            int last = next;
            if (i >= numbers.Length)
            {
                if (lastpos[last] > 0)
                    next = i - lastpos[last];
                else
                    next = 0;
            }
            else
            {
                next = numbers[i];
            }
            lastpos[last] = i;
            // W($"{next}");
        }
        return next;
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