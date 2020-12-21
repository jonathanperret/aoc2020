using System;
using System.IO;
using System.Linq;
using Sprache;
using System.Numerics;
using static MoreLinq.Extensions.ToDelimitedStringExtension;
using static MoreLinq.Extensions.ForEachExtension;
using static MoreLinq.Extensions.PartitionExtension;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using static Util;
using System.Collections.Immutable;

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

    public static (int, string) Solve(string[] lines)
    {
        var lists = lines.Select(Parse);

        var mayBeContainedIn = lists
            .SelectMany(list => list.allergens
                .Select(allergen => (allergen, ingredients: list.ingredients.ToImmutableHashSet())))
            .GroupBy(keySelector: t => t.allergen,
                elementSelector: t => t.ingredients,
                resultSelector: (allergen, ingredientLists) => (
                    allergen,
                    ingredients: ingredientLists.Aggregate((set1, set2) => set1.Intersect(set2))
                ))
            .ToImmutableDictionary(t => t.allergen, t => t.ingredients);

        W("---");
        mayBeContainedIn.ForEach(kv =>
        {
            W($"{kv.Key} may be contained in {kv.Value.ToDelimitedString(", ")}");
        });
        W("---");

        var mustBeContainedIn = ImmutableDictionary.Create<Allergen, Ingredient>();

        while (mayBeContainedIn.Count > 0)
        {
            var (certain, incertain) = mayBeContainedIn.Partition(kv => kv.Value.Count == 1);
            mustBeContainedIn = mustBeContainedIn.SetItems(
                certain.Select(kv => new KeyValuePair<Allergen, Ingredient>(kv.Key, kv.Value.First()))
            );
            mayBeContainedIn = incertain
                .Select(kv => (allergen: kv.Key, ingredients: kv.Value.Except(mustBeContainedIn.Values)))
                .ToImmutableDictionary(t => t.allergen, t => t.ingredients);
        }

        int safeIngredientCount = 0;

        foreach (var list in lists)
        {
            foreach (var ing in list.ingredients)
            {
                if (!mustBeContainedIn.ContainsValue(ing))
                {
                    // W($"{ing} is safe");
                    safeIngredientCount++;
                }
            }
        }

        string canonicalList = mustBeContainedIn.OrderBy(kv => kv.Key.name)
                .Select(kv => kv.Value.name)
                .ToDelimitedString(",");

        return (safeIngredientCount, canonicalList);
    }

    static void Main(string[] args)
    {
        var lines = File.ReadAllLines("input.txt");

        var result = Solve(lines);
        W($"{result}");
    }
}
