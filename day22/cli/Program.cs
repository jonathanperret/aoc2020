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
    public static long Score(IEnumerable<long> deck)
    {
        return deck.Reverse().Select((c, i) => c * (i + 1)).Sum();
    }

    public static long Part1(string text)
    {
        var lines = text.Trim().Split("\n");
        var groups = lines.Split("").ToArray();

        var deck1 = new Queue<long>(groups[0].Skip(1).Select(long.Parse).ToArray());
        var deck2 = new Queue<long>(groups[1].Skip(1).Select(long.Parse).ToArray());

        while (deck1.Count > 0 && deck2.Count > 0)
        {
            W($"player 1's deck: {deck1.ToDelimitedString(", ")}");
            W($"player 2's deck: {deck2.ToDelimitedString(", ")}");

            var card1 = deck1.Dequeue();
            var card2 = deck2.Dequeue();
            W($"player 1 plays {card1}");
            W($"player 2 plays {card2}");

            if (card1 > card2)
            {
                W($"player 1 wins round");
                deck1.Enqueue(card1);
                deck1.Enqueue(card2);
            }
            else
            {
                W($"player 2 wins round");
                deck2.Enqueue(card2);
                deck2.Enqueue(card1);
            }
        }
        W($"player 1's deck: {deck1.ToDelimitedString(", ")}");
        W($"player 2's deck: {deck2.ToDelimitedString(", ")}");

        var winnerDeck = deck1.Count > 0 ? deck1 : deck2;

        return Score(winnerDeck);
    }

    static void Main(string[] args)
    {
        var text = File.ReadAllText("input.txt");

        var result = Part1(text);
        W($"{result}");
    }
}
