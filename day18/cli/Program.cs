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
        var states = new Stack<(long sum, long product, char op)>();
        char op = '+';
        long sum = 0;
        long product = 1;
        foreach (char c in expr.Replace(" ", ""))
        {
            switch (c)
            {
                default:
                    var value = long.Parse(c.ToString());
                    if (op == '+') sum += value; else product = value;
                    break;
                case '*':
                    product *= sum;
                    sum = 0;
                    op = '+';
                    break;
                case '+':
                    op = c;
                    break;
                case '(':
                    states.Push((sum, product, op));
                    op = '+';
                    sum = 0;
                    product = 1;
                    break;
                case ')':
                    value = product * sum;
                    (sum, product, op) = states.Pop();
                    if (op == '+') sum += value; else product = value;
                    break;
            }

        }
        return product * sum;
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