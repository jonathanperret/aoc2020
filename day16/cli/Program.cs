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
        var positionSetByFieldIndex = tickets[0].Select((x) => Enumerable.ToHashSet(Enumerable.Range(0, fieldCount))).ToArray();

        foreach (var ticket in validTickets)
        {
            for (int position = 0; position < ticket.Length; position++)
            {
                for (int fieldIndex = 0; fieldIndex < fieldCount; fieldIndex++)
                {
                    if (!fields[fieldIndex].Any(range => ticket[position] >= range.min && ticket[position] <= range.max))
                    {
                        W($"field {fieldIndex} can't be in position {position}");
                        positionSetByFieldIndex[fieldIndex].Remove(position);
                    }
                }
            }
        }

        W(positionSetByFieldIndex.Select((s, i) => $"{i}:{s.ToDelimitedString(",")}").ToDelimitedString("; "));

        while (positionSetByFieldIndex.Any(s => s.Count > 1))
        {
            for (int fieldIndex = 0; fieldIndex < fieldCount; fieldIndex++)
            {
                if (positionSetByFieldIndex[fieldIndex].Count == 1)
                {
                    int fieldPos = positionSetByFieldIndex[fieldIndex].First();
                    W($"found position for field {fieldIndex}: {fieldPos}");
                    for (int otherFieldIndex = 0; otherFieldIndex < fieldCount; otherFieldIndex++)
                    {
                        if (otherFieldIndex != fieldIndex)
                        {
                            positionSetByFieldIndex[otherFieldIndex].Remove(fieldPos);
                        }
                    }

                }
            }
        }

        W(positionSetByFieldIndex.Select((s, i) => $"{i}:{s.ToDelimitedString(",")}").ToDelimitedString("; "));

        var fieldPositions = positionSetByFieldIndex
            .Select((s, j) => (position: s.First(), index: j))
            .ToArray();

        var departurePos = fieldPositions
            .Where((position, fieldIndex) => fieldNames[fieldIndex].StartsWith("departure"))
            .Select(field => field.position);

        W($"departure pos: {departurePos.ToDelimitedString(",")}");

        var mydeparture = departurePos.Select(position => (long)myticket[position]);

        W($"my departure fields: {mydeparture.ToDelimitedString(",")}");

        return mydeparture.Product();
    }

    static void Main(string[] args)
    {
        var lines = File.ReadAllLines("input.txt");

        var result = Solve(lines);
        W($"{result}");
    }
}