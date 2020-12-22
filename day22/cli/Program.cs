using System;
using System.IO;
using System.Linq;
using Sprache;
using System.Numerics;
using MoreLinq;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using static Util;
using System.Collections.Immutable;

public static class Program
{
    public static int Score(IEnumerable<int> deck)
    {
        return deck.Reverse().Select((c, i) => c * (i + 1)).Sum();
    }

    public class GameState
    {
        public ImmutableQueue<int> deck1;
        public ImmutableQueue<int> deck2;
        public string GameStr => $"{deck1.ToDelimitedString(",")}|{deck2.ToDelimitedString(",")}";

        public override int GetHashCode()
        {
            int hash = 0;
            foreach (int c in deck1)
            {
                hash = 31 * hash + c;
            }
            hash = 31 * hash;
            foreach (int c in deck2)
            {
                hash = 31 * hash + c;
            }
            // W($"Hashed {GameStr} to {hash}");
            return hash;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            GameState other = obj as GameState;

            return deck1.SequenceEqual(other.deck1) && deck2.SequenceEqual(other.deck2);
        }
    }

    public static int Part2(string text)
    {
        var solved = new Dictionary<GameState, (int, ImmutableQueue<int>)>();

        (int, ImmutableQueue<int>) Game(GameState initialState, int depth)
        {
            if (solved.TryGetValue(initialState, out var initialResult))
            {
                // W($"found cached game, winner is {initialResult.Item1}");
                return initialResult;
            }
            int winner = -1;
            int round = 1;
            var seen = new HashSet<GameState>();
            GameState state = initialState;
            while (!(state.deck1.IsEmpty || state.deck2.IsEmpty))
            {
                if (solved.TryGetValue(state, out var result))
                {
                    // W($"found cached game, winner is {result.Item1}");
                    if (round > 1)
                    {
                        // does not really speed up anything!
                        // foreach (string s in seen) if (!solved.ContainsKey(s)) solved[s] = result;

                        solved[state] = result;
                    }
                    // W($"{solved.Count} games solved");
                    return result;
                }

                // W($"game {depth} round {round}: player 1's deck: {state.deck1.ToDelimitedString(", ")}");
                // W($"game {depth} round {round}: player 2's deck: {state.deck2.ToDelimitedString(", ")}");

                if (!seen.Add(state))
                {
                    // W($"game {depth} round {round}: infinite game detected!");
                    winner = 1;
                    break;
                }

                var newdeck1 = state.deck1.Dequeue(out var card1);
                var newdeck2 = state.deck2.Dequeue(out var card2);
                // W($"game {depth} round {round}: player 1 plays {card1}");
                // W($"game {depth} round {round}: player 2 plays {card2}");

                if (card1 <= newdeck1.Count() && card2 <= newdeck2.Count())
                {
                    var subdeck1 = ImmutableQueue.CreateRange<int>(newdeck1.Take(card1));
                    var subdeck2 = ImmutableQueue.CreateRange<int>(newdeck2.Take(card2));

                    var substate = new GameState() { deck1 = subdeck1, deck2 = subdeck2 };

                    (winner, _) = Game(substate, depth + 1);
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
                    state = new GameState()
                    {
                        deck1 = newdeck1.Enqueue(card1).Enqueue(card2),
                        deck2 = newdeck2,
                    };
                }
                else
                {
                    state = new GameState()
                    {
                        deck1 = newdeck1,
                        deck2 = newdeck2.Enqueue(card2).Enqueue(card1),
                    };
                }

                round++;
            }

            // W($"game {depth}: player {winner} wins game");
            var winnerDeck = state.deck2.IsEmpty ? state.deck1 : state.deck2;
            var finalResult = (winner, winnerDeck);
            // does not really speed up anything!
            // foreach (string s in seen) if (!solved.ContainsKey(s)) solved[s] = finalResult;
            solved[initialState] = finalResult;
            // W($"{solved.Count} games solved");
            return finalResult;
        }


        var lines = text.Trim().Split("\n");
        var groups = lines.Split("").ToArray();

        var deck1 = ImmutableQueue.Create<int>(groups[0].Skip(1).Select(int.Parse).ToArray());
        var deck2 = ImmutableQueue.Create<int>(groups[1].Skip(1).Select(int.Parse).ToArray());
        var state = new GameState() { deck1 = deck1, deck2 = deck2 };

        var (winner, winnerDeck) = Game(state, depth: 0);

        return Score(winnerDeck);
    }

    static void Main(string[] args)
    {
        var text = File.ReadAllText("input.txt");

        var result = Part2(text);
        W($"{result}");
    }
}
