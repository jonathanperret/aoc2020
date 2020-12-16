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
    public static int CountBits(int value)
    {
        int count = 0;
        while (value != 0)
        {
            count++;
            value &= value - 1;
        }
        return count;
    }

    public static int FindBit(int value)
    {
        int bitval = 1;
        int i = 0;
        while ((value & bitval) == 0)
        {
            i++;
            bitval *= 2;
        }
        return i;
    }

    public static long Solve(string[] lines)
    {
        var groups = lines.Split("").ToArray();

        var fields = groups[0].Select(fieldstr =>
        {
            return fieldstr.Split(": ")[1].Split(" or ")
                .Select(rangestr => rangestr.Split("-").Select(int.Parse).Fold((min, max) => (min: min, max: max))
                ).ToArray();
        }).ToArray();

        var fieldNames = groups[0].Select(fieldstr =>
        {
            return fieldstr.Split(": ")[0];
        }).ToArray();

        //W(fields.ToDelimitedString(","));

        var myticket = groups[1].ElementAt(1).Split(",").Select(int.Parse).ToArray();

        var tickets = groups[2].Skip(1).Select(
            ticketstr => ticketstr.Split(",").Select(int.Parse).ToArray()
        ).ToArray();

        //        W(tickets.Select(t => "(" + t.ToDelimitedString(",") + ")").ToDelimitedString(","));
        W(myticket.ToDelimitedString(","));

        var validTickets = Enumerable.Append(tickets, myticket).Where(values =>
             values.All(value => fields.Any(ranges => ranges.Any(range =>
                 value >= range.min && value <= range.max
             )))
        );

        W(validTickets.Select(t => "(" + t.ToDelimitedString(",") + ")").ToDelimitedString(","));

        int fieldCount = tickets[0].Length;
        int[] fieldPosConstraints = new int[fieldCount];
        for (int i = 0; i < fieldCount; i++)
        {
            fieldPosConstraints[i] = (int)Math.Pow(2, fieldCount) - 1;
        }

        foreach (var ticket in validTickets)
        {
            int bitval = 1;
            for (int i = 0; i < ticket.Length; i++)
            {
                for (int j = 0; j < fieldCount; j++)
                {
                    if (!fields[j].Any(range => ticket[i] >= range.min && ticket[i] <= range.max))
                    {
                        W($"field {j} can't be in position {i}, removing {bitval}");
                        fieldPosConstraints[j] &= ~bitval;
                        W($"{Convert.ToString(fieldPosConstraints[j], 2).PadLeft(fieldCount, '0')}");
                    }
                }
                bitval *= 2;
            }
        }

        while (fieldPosConstraints.Any(c => CountBits(c) > 1))
        {
            for (int j = 0; j < fieldCount; j++)
            {
                if (CountBits(fieldPosConstraints[j]) == 1)
                {
                    int fieldPos = FindBit(fieldPosConstraints[j]);
                    W($"found position for field {j}: {fieldPos}");
                    for (int k = 0; k < fieldCount; k++)
                    {
                        if (k != j)
                        {
                            fieldPosConstraints[k] &= ~fieldPosConstraints[j];
                        }
                    }

                }
            }
        }

        W(fieldPosConstraints.Select(c => Convert.ToString(c, 2).PadLeft(fieldCount, '0')).ToDelimitedString(","));

        var departurePos = fieldPosConstraints
            .Select((c, j) => (constraint: c, index: j))
            .Where(field => fieldNames[field.index].StartsWith("departure"))
            .Select(field => FindBit(field.constraint));

        W($"departure pos: {departurePos.ToDelimitedString(",")}");

        var mydeparture = departurePos.Select(j => (long)myticket[j]);

        W($"my departure fields: {mydeparture.ToDelimitedString(",")}");

        return mydeparture.Aggregate((a, b) => a * b);
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