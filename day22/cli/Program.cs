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

    public static long Part2(string text)
    {
        var solved = new Dictionary<string, (int, Queue<long>)>();

        (int, Queue<long>) Game(Queue<long> deck1, Queue<long> deck2, int depth)
        {
            int winner = -1;
            int round = 1;
            var seen = new HashSet<string>();
            string initialGameStr = GetGameStr(deck1, deck2);
            string gameStr = "";
            while (deck1.Count > 0 && deck2.Count > 0)
            {
                gameStr = GetGameStr(deck1, deck2);
                if (solved.TryGetValue(gameStr, out var result))
                {
                    // W($"found cached game, winner is {result.Item1}");
                    if (round > 1)
                    {
                        // does not really speed up anything!
                        foreach (string s in seen) if (!solved.ContainsKey(s)) solved[s] = result;

                        solved[initialGameStr] = result;
                    }
                    // W($"{solved.Count} games solved");
                    return result;
                }

                // W($"game {depth} round {round}: player 1's deck: {deck1.ToDelimitedString(", ")}");
                // W($"game {depth} round {round}: player 2's deck: {deck2.ToDelimitedString(", ")}");

                if (seen.Contains(gameStr))
                {
                    // W($"game {depth} round {round}: infinite game detected!");
                    winner = 1;
                    break;
                }
                seen.Add(gameStr);

                var card1 = deck1.Dequeue();
                var card2 = deck2.Dequeue();
                // W($"game {depth} round {round}: player 1 plays {card1}");
                // W($"game {depth} round {round}: player 2 plays {card2}");

                if (card1 <= deck1.Count && card2 <= deck2.Count)
                {
                    var subdeck1 = new Queue<long>(deck1.Take((int)card1));
                    var subdeck2 = new Queue<long>(deck2.Take((int)card2));

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
                    deck1.Enqueue(card1);
                    deck1.Enqueue(card2);
                }
                else
                {
                    // W($"game {depth} round {round}: player 2 wins round");
                    deck2.Enqueue(card2);
                    deck2.Enqueue(card1);
                }

                round++;
            }

            static string GetGameStr(Queue<long> deck1, Queue<long> deck2)
            {
                return $"{deck1.ToDelimitedString(",")}|{deck2.ToDelimitedString(",")}";
            }

            // W($"game {depth}: player {winner} wins game");
            var winnerDeck = deck1.Count > 0 ? deck1 : deck2;
            var finalResult = (winner, winnerDeck);
            // does not really speed up anything!
            // foreach (string s in seen) if (!solved.ContainsKey(s)) solved[s] = finalResult;
            solved[initialGameStr] = finalResult;
            // W($"{solved.Count} games solved");
            return finalResult;
        }


        var lines = text.Trim().Split("\n");
        var groups = lines.Split("").ToArray();

        var deck1 = new Queue<long>(groups[0].Skip(1).Select(long.Parse).ToArray());
        var deck2 = new Queue<long>(groups[1].Skip(1).Select(long.Parse).ToArray());

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
