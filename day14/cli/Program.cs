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
    public static long solve(string[] input)
    {
        long[] memory = new long[100000];

        string maskstr = "";
        foreach (string line in input)
        {
            if (line.StartsWith("mask"))
            {
                maskstr = line.Split(" = ")[1];
            }
            else
            {
                string addrstr = line.Split(" = ")[0];
                addrstr = addrstr.Substring(4, addrstr.Length - 5);
                long addr = long.Parse(addrstr);
                string valuestr = line.Split(" = ")[1];
                long value = long.Parse(valuestr);
                long orgvalue = value;

                long bitval = 1;
                for (int i = maskstr.Length - 1; i >= 0; i--)
                {
                    char maskchar = maskstr[i];

                    switch (maskchar)
                    {
                        case '0':
                            value = value & (~bitval);
                            break;
                        case '1':
                            value = value | (bitval);
                            break;
                        default:
                            break;
                    }

                    bitval *= 2;
                }
                W($"{orgvalue} -> {value}");
                memory[addr] = value;
            }

        }

        return memory.Sum();
    }

    static void Main(string[] args)
    {
        var text = File.ReadAllText("input.txt");
        var lines = File.ReadAllLines("input.txt").ToArray();
        // var groups = lines.Split("");
        // var numbers = lines.Select(int.Parse).ToArray();
        // var blocks = numbers.Window(26);
        // int max = numbers.Max();
        // int min = numbers.Min();

        var result = solve(lines);
        W($"{result}");
    }
}