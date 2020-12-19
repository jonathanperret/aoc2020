using System;
using System.IO;
using System.Linq;
using Sprache;
using MoreLinq;
using System.Collections.Generic;
using static Util;

public static class Program
{
    public static int Solve(string[] lines)
    {
        var groups = lines.Split("").ToArray();
        var rules = groups[0].Select(line => line.Split(": ")).ToDictionary(
            pair => int.Parse(pair[0]),
            pair => pair[1]
        );
        var messages = groups[1].ToArray();

        rules[8] = "42 | 42 8";
        rules[11] = "42 31 | 42 11 31";

        IEnumerable<string> Only(string s) => new[] { s };

        IEnumerable<string> Match(string alternatives, string rest)
        {
            return alternatives.Split(" | ").SelectMany(alt =>
                alt.Split(" ").Aggregate(
                    Only(rest),
                    (rests, term) => rests.SelectMany(rest =>
                    {
                        if (term[0] == '"')
                        {
                            if (rest[0] == term[1])
                                return Only(rest.Substring(1));
                            else
                                return Array.Empty<string>();
                        }
                        return Match(rules[int.Parse(term)], rest);
                    })
                )
            );
        }

        return messages.Where(m => Match(rules[0], m + "$").Contains("$")).Count();
    }

    static void Main(string[] args)
    {
        var lines = File.ReadAllLines("input.txt");
        var result = Solve(lines);
        W($"{result}");
    }
}