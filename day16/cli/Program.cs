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
    public static int Solve(string[] lines)
    {
        var groups = lines.Split("").ToArray();

        var fields = groups[0].SelectMany(fieldstr =>
        {
            return fieldstr.Split(": ")[1].Split(" or ")
                .Select(rangestr => rangestr.Split("-").Select(int.Parse).Fold((min, max) => (min: min, max: max))
                ).ToArray();
        }).ToArray();

        W(fields.ToDelimitedString(","));

        var tickets = groups[2].Skip(1).Select(
            ticketstr => ticketstr.Split(",").Select(int.Parse).ToArray()
        ).ToArray();

        W(tickets.Select(t => "(" + t.ToDelimitedString(",") + ")").ToDelimitedString(","));

        var invalidSum = tickets.SelectMany(values =>
            values.Where(value => !fields.Any(field =>
                value >= field.min && value <= field.max
            ))
        ).Sum();

        return invalidSum;
    }

    static void Main(string[] args)
    {
        var text = File.ReadAllText("input.txt");
        var lines = File.ReadAllLines("input.txt");
        // var numbers = lines.Select(int.Parse).ToArray();
        // var blocks = numbers.Window(26);
        // int max = numbers.Max();
        // int min = numbers.Min();

        var result = Solve(lines);
        W($"{result}");
    }
}