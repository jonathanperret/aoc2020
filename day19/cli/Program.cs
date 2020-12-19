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
        var rules = Enumerable.ToDictionary(groups[0].Select(line => line.Split(": ")), pair => pair[0], pair => pair[1]);
        var messages = groups[1].ToArray();

        string translate(string rule)
        {
            if (rule[0] == '"')
            {
                return rule[1].ToString();
            }
            var alts = rule.Split(" | ");
            return "(" + alts.Select(alt =>
            {
                return alt.Split(" ").Select(id => translate(rules[id])).ToDelimitedString("");
            }).ToDelimitedString("|") + ")";
        }

        string pattern = $"^{translate(rules["0"])}$";

        W(pattern);

        var re = new Regex(pattern);

        W(messages.Where(m => re.IsMatch(m)).ToDelimitedString("\n"));

        return messages.Count(m => re.IsMatch(m));
    }

    static void Main(string[] args)
    {
        var text = File.ReadAllText("input.txt");
        var lines = text.Trim().Split("\n");

        var result = Solve(lines);
        W($"{result}");
    }
}