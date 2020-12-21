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
    public record Ingredient(string name);
    public record Allergen(string name);
    public record IngredientList(Ingredient[] ingredients, Allergen[] allergens);
    public static IngredientList Parse(string line)
    {
        var (ing, allerg) = line.Split(" (contains ", 2);

        return new IngredientList(ing.Split(" ").Select(n => new Ingredient(n)).ToArray(), allerg[..^1].Split(", ").Select(n => new Allergen(n)).ToArray());
    }

    public static int Part1(string[] lines)
    {
        var groups = lines.Split("");
        var lists = lines.Select(Parse);

        Dictionary<Ingredient, HashSet<Allergen>> mayContain = new();
        Dictionary<Allergen, HashSet<Ingredient>> mayBeContainedIn = new();
        Dictionary<Ingredient, Allergen> mustContain = new();

        foreach (var list in lists)
        {
            foreach (var ing in list.ingredients)
            {
                if (!mayContain.TryGetValue(ing, out var allergenSet))
                {
                    allergenSet = new();
                    mayContain.Add(ing, allergenSet);
                }
                foreach (var allerg in list.allergens)
                {
                    allergenSet.Add(allerg);
                }
            }
            foreach (var allerg in list.allergens)
            {
                if (!mayBeContainedIn.TryGetValue(allerg, out var ingSet))
                {
                    ingSet = new(list.ingredients);
                    mayBeContainedIn.Add(allerg, ingSet);
                }
                else
                {
                    ingSet.IntersectWith(list.ingredients);
                    W($"{allerg} may now be only in {ingSet.ToDelimitedString(", ")}");
                }
            }
        }

        // mayContain.ForEach(kv =>
        // {
        //     W($"{kv.Key} may contain {kv.Value.ToDelimitedString(", ")}");
        // });
        W("---");
        mayBeContainedIn.ForEach(kv =>
        {
            W($"{kv.Key} may be contained in {kv.Value.ToDelimitedString(", ")}");
        });
        W("---");


        bool again = true;
        HashSet<Allergen> found = new();
        while (again)
        {
            again = false;
            foreach (Allergen allerg in mayBeContainedIn.Keys)
            {
                if (found.Contains(allerg)) continue;
                var ingSet = mayBeContainedIn[allerg];
                if (ingSet.Count == 1)
                {
                    var ing = ingSet.First();
                    W($"{allerg} must be in {ing}");
                    foreach (var otherAllerg in mayBeContainedIn.Keys)
                    {
                        if (otherAllerg == allerg || found.Contains(otherAllerg)) continue;
                        mayBeContainedIn[otherAllerg].Remove(ing);
                    }
                    mustContain[ing] = allerg;

                    found.Add(allerg);
                    again = true;
                }
            }
        }

        W("---");
        mayBeContainedIn.ForEach(kv =>
        {
            W($"{kv.Key} may be contained in {kv.Value.ToDelimitedString(", ")}");
        });
        W("---");

        // mayContain.ForEach(kv =>
        // {
        //     W($"{kv.Key} may contain {kv.Value.ToDelimitedString(", ")}");
        // });
        // mayBeContained.ForEach(kv =>
        // {
        //     W($"{kv.Key} may be contained in {kv.Value.ToDelimitedString(", ")}");
        // });

        int safeIngredientCount = 0;

        foreach (var list in lists)
        {
            foreach (var ing in list.ingredients)
            {
                if (!mustContain.ContainsKey(ing))
                {
                    // W($"{ing} is safe");
                    safeIngredientCount++;
                }
            }
        }

        W(mustContain.OrderBy(kv => kv.Value.name)
        .Select(kv => kv.Key.name)
        .ToDelimitedString(","));


        return safeIngredientCount;
    }

    static void Main(string[] args)
    {
        var lines = File.ReadAllLines("input.txt");
        // var numbers = lines.Select(int.Parse).ToArray();
        // var blocks = numbers.Window(26);
        // int max = numbers.Max();
        // int min = numbers.Min();

        var result = Part1(lines);
        W($"{result}");
    }
}
