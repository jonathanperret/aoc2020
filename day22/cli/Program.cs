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
    public static int Score(string deck)
    {
        return deck.Reverse().Select((c, i) => (int)c * (i + 1)).Sum();
    }

    public static int Part2(string text)
    {
        var solved = new Dictionary<(string, string), int>();

        (int winner, string winnerDeck) Game(string deck1, string deck2, int depth)
        {
            int winner = -1;
            int round = 1;
            var seen = new HashSet<(string, string)>();
            var initialGameStr = GetGameStr(deck1, deck2);
            while (deck1.Length > 0 && deck2.Length > 0)
            {
                var gameStr = GetGameStr(deck1, deck2);
                if (solved.TryGetValue(gameStr, out var result))
                {
                    // W($"found cached game, winner is {result.Item1}");
                    if (round > 1)
                    {
                        // does not really speed up anything!
                        // foreach (string s in seen) if (!solved.ContainsKey(s)) solved[s] = result;

                        solved[initialGameStr] = result;
                    }
                    // W($"{solved.Count} games solved");
                    return (result, null);
                }

                // W($"game {depth} round {round}: player 1's deck: {deck1.Select(c => (int)c).ToDelimitedString(", ")}");
                // W($"game {depth} round {round}: player 2's deck: {deck2.Select(c => (int)c).ToDelimitedString(", ")}");

                if (seen.Contains(gameStr))
                {
                    // W($"game {depth} round {round}: infinite game detected!");
                    winner = 1;
                    break;
                }
                seen.Add(gameStr);

                var card1 = deck1[0];
                var card2 = deck2[0];
                // W($"game {depth} round {round}: player 1 plays {(int)card1}");
                // W($"game {depth} round {round}: player 2 plays {(int)card2}");

                if (card1 < deck1.Length && card2 < deck2.Length)
                {
                    var subdeck1 = deck1[1..(card1 + 1)];
                    var subdeck2 = deck2[1..(card2 + 1)];

                    (winner, _) = Game(subdeck1, subdeck2, depth + 1);
                }
                else
                {
                    if (card1 > card2)
                    {
                        winner = 1;
                    }
                    else
                    {
                        winner = 2;
                    }
                }
                if (winner == 1)
                {
                    deck1 = deck1[1..] + card1 + card2;
                    deck2 = deck2[1..];
                }
                else
                {
                    deck1 = deck1[1..];
                    deck2 = deck2[1..] + card2 + card1;
                }

                round++;
            }

            static (string, string) GetGameStr(string deck1, string deck2)
            {
                return (deck1, deck2);
            }

            // W($"game {depth}: player {winner} wins game");

            // does not really speed up anything!
            // foreach (string s in seen) if (!solved.ContainsKey(s)) solved[s] = finalResult;
            solved[initialGameStr] = winner;
            // W($"{solved.Count} games solved");
            return (winner, winner == 1 ? deck1 : deck2);
        }


        var lines = text.Trim().Split("\n");
        var groups = lines.Split("").ToArray();

        var deck1 = groups[0].Skip(1).Select(int.Parse).Select(i => (char)i).ToDelimitedString("");
        var deck2 = groups[1].Skip(1).Select(int.Parse).Select(i => (char)i).ToDelimitedString("");

        var (winner, winnerDeck) = Game(deck1, deck2, depth: 0);

        return Score(winnerDeck);
    }

    static void Main(string[] args)
    {
        var text = File.ReadAllText("input.txt");

        var result = Part2(text);
        W($"{result}");
    }
}
