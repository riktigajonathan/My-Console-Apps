using System;
using System.Collections.Generic;
using System.Linq;

namespace BridgeSolver;

internal class Deck
{
    protected List<Card> cards = new();

    public Deck()
    {
        Init();
    }

    private void Init()
    {
        foreach (SUIT suit in Enum.GetValues<SUIT>())
        {
            foreach (RANK rank in Enum.GetValues<RANK>())
            {
                cards.Add(new Card(suit, rank));
            }
        }
    }

    public Card? DrawCard()
    {
        if (cards.Count > 0)
        {
            Card card = cards[0];
            cards.RemoveAt(0);
            return card;
        }
        else
        {
            // no cards...
            return null;
        }
    }

    public void Shuffle()
    {
        Random rng = new Random();
        int n = cards.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            Card value = cards[k];
            cards[k] = cards[n];
            cards[n] = value;
        }
    }

    public void Sort()
    {
        cards = cards.OrderBy(card => card.Suit).ThenBy(card => card.Rank).ToList();
    }

    public void Print()
    {
        foreach (Card card in cards)
        {
            Console.BackgroundColor = card.GetConsoleColor();

            string rank = "";
            if ((int)card.Rank < 10)
            {
                rank = ((int)card.Rank).ToString();
            }
            else
            {
                rank = card.Rank.ToString()[0].ToString();
            }

            Console.Write($"{card.Suit.ToString()[0].ToString().ToLower()}{rank}");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write(" ");
        }
    }

    public List<Card> GetCards() => cards;
}