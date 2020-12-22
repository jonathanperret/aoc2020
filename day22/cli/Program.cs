using System;
using System.IO;
using System.Linq;
using Sprache;
using System.Numerics;
using MoreLinq;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Collections;
using static Util;

using Card = System.Byte;

#nullable enable
public static class Program
{
    // public sealed record Card(int value)
    // {
    //     public static implicit operator Card(int value) => new Card(value);
    //     public static implicit operator int(Card card) => card.value;
    // }

    private static Memory<Card> AllocDeck(int n)
    {
        return new Card[n];
    }

    public sealed record Deck(Memory<Card> cards)
    {
        public static implicit operator Deck(Card[] cards) => new Deck(cards.AsMemory());
        public int Count => cards.Length;
        public (Card, Deck) Pick() => (cards.Span[0], new Deck(cards.Slice(1)));
        public Deck Take(int n) => new Deck(cards.Slice(1, n));
        public Deck Append(Card card1, Card card2)
        {
            var newmem = AllocDeck(cards.Length + 2);
            cards.CopyTo(newmem);
            newmem.Span[cards.Length] = card1;
            newmem.Span[cards.Length + 1] = card2;
            return new Deck(newmem);
        }
        public int Score => cards.ToArray().Reverse().Select((c, i) => (int)c * (i + 1)).Sum();

        public override int GetHashCode()
        {
            int hash = 0;
            var data = cards.Span;
            for (int i = 0; i < cards.Length; i++) hash = hash * 31 + (int)data[i];
            return hash;
        }

        public bool Equals(Deck? other)
        {
            if (other == null)
            {
                return false;
            }
            return cards.Span.SequenceEqual(other.cards.Span);
        }
    }

    public sealed record GameState(Deck deck1, Deck deck2)
    {
    }

    public static int Part2(string text)
    {
        (int winner, Deck winnerDeck) Game(GameState state, int depth)
        {
            int winner = -1;
            int round = 1;
            var seen = new HashSet<GameState>();
            while (state.deck1.Count > 0 && state.deck2.Count > 0)
            {
                // W($"game {depth} round {round}");
                // W($"game {depth} round {round}: player 1's deck: {deck1.Select(c => (int)c).ToDelimitedString(", ")}");
                // W($"game {depth} round {round}: player 2's deck: {deck2.Select(c => (int)c).ToDelimitedString(", ")}");

                if (seen.Contains(state))
                {
                    // W($"game {depth} round {round}: infinite game detected!");
                    winner = 1;
                    break;
                }
                seen.Add(state);

                var (card1, rest1) = state.deck1.Pick();
                var (card2, rest2) = state.deck2.Pick();
                // W($"game {depth} round {round}: player 1 plays {(int)card1}");
                // W($"game {depth} round {round}: player 2 plays {(int)card2}");

                if (card1 < state.deck1.Count && card2 < state.deck2.Count)
                {
                    var subdeck1 = state.deck1.Take((int)card1);
                    var subdeck2 = state.deck2.Take((int)card2);
                    var substate = new GameState(subdeck1, subdeck2);

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
                    state = new GameState(
                        rest1.Append(card1, card2),
                        rest2
                    );
                }
                else
                {
                    state = new GameState(
                        rest1,
                        rest2.Append(card2, card1)
                    );
                }

                round++;
            }

            // W($"game {depth}: player {winner} wins game");
            return (winner, winner == 1 ? state.deck1 : state.deck2);
        }


        var lines = text.Trim().Split("\n");
        var groups = lines.Split("").ToArray();

        var deck1 = groups[0].Skip(1).Select(int.Parse).Select(i => (Card)i).ToArray();
        var deck2 = groups[1].Skip(1).Select(int.Parse).Select(i => (Card)i).ToArray();
        var initState = new GameState(deck1, deck2);

        var (winner, winnerDeck) = Game(initState, depth: 0);

        return winnerDeck.Score;
    }

    static void Main(string[] args)
    {
        var text = File.ReadAllText("input.txt");

        var result = Part2(text);
        W($"{result}");
    }
}
