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
        var memory = new Dictionary<long, long>();

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

                long bitval = 1;

                var addrlist = new List<long>();
                addrlist.Add(addr);
                for (int i = maskstr.Length - 1; i >= 0; i--)
                {
                    char maskchar = maskstr[i];

                    switch (maskchar)
                    {
                        case '0':
                            break;
                        case '1':
                            addrlist = addrlist.Select(a => a | bitval).ToList();
                            break;
                        default:
                            addrlist = addrlist.Select(a => a | bitval).Concat(addrlist.Select(a => a & (~bitval))).ToList();
                            break;
                    }

                    bitval *= 2;
                }
                W($"Writing to {addrlist.ToDelimitedString(",")}");
                foreach (var a in addrlist)
                {
                    memory[a] = value;
                }

            }

        }

        return memory.Values.Sum();
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