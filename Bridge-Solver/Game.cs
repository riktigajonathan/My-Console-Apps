using System;
using System.Collections.Generic;

namespace BridgeSolver;

internal class Game
{
    private Deck deck;

    Dictionary<Cardinal, Player> players = new();

    public Game()
    {
        deck = new Deck();
        deck.Shuffle();

        InitPlayers();
    }

    private void InitPlayers()
    {
        foreach (var cardinal in Enum.GetValues<Cardinal>())
        {
            List<Card> cards = new();

            for (int i = 0; i < 13; i++)
            {
                var card = deck.DrawCard();

                if (card != null)
                {
                    cards.Add(card);
                }
                else
                {
                    break;
                }
            }

            Player player = new(cards);

            players[cardinal] = player;
        }
    }

    public void PrintPlayers()
    {
        foreach (var player in players)
        {
            Console.Write($"{player.Key.ToString().PadRight(5)}:");
            player.Value.Print();
            Console.WriteLine();
        }
    }
}

public enum Cardinal
{
    North,
    East,
    South,
    West
}