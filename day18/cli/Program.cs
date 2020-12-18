using System;
using System.IO;
using System.Linq;
using Sprache;
using System.Numerics;
using MoreLinq;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using static Util;
using System.Diagnostics;

public static class Program
{
    public static long Compute(string expr)
    {
        var totals = new Stack<long>();
        var ops = new Stack<char>();
        char op = '+';
        long top = 0;
        foreach (char c in expr.Replace(" ", ""))
        {
            switch (c)
            {
                case '(':
                    totals.Push(top);
                    ops.Push(op);
                    op = '+';
                    top = 0;
                    break;
                case ')':
                    long prevtop = totals.Pop();
                    op = ops.Pop();
                    if (op == '*') top = prevtop * top; else top = prevtop + top;
                    break;
                case '*':
                    op = c;
                    break;
                case '+':
                    op = c;
                    break;
                default:
                    var value = long.Parse(c.ToString());
                    if (op == '*') top *= value; else top += value;
                    break;
            }

        }
        return top;
    }
    public static long Solve(string[] lines)
    {
        return lines.Select(Compute).Sum();
    }

    static void Main(string[] args)
    {
        var text = File.ReadAllText("input.txt");
        var lines = File.ReadAllLines("input.txt");
        var groups = lines.Split("");
        // var numbers = lines.Select(int.Parse).ToArray();
        // var blocks = numbers.Window(26);
        // int max = numbers.Max();
        // int min = numbers.Min();

        var result = Solve(lines);
        W($"{result}");
    }
}