using System;
using System.Collections.Generic;
using System.Text;

internal class Player : Deck
{
    public Player(List<Card> cards)
    {
        this.cards.Clear();

        foreach (var card in cards)
        {
            this.cards.Add(card);
        }

        Sort();
    }

    public void AddCard(Card? card)
    {
        if (card != null)
            cards.Add(card);
    }
}
