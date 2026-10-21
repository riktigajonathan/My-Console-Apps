using System;

namespace BridgeSolver;

internal class Card
{
    public SUIT Suit { get; private set; }
    public RANK Rank { get; private set; }

    public Card(SUIT suit, RANK rank)
    {
        Suit = suit;
        Rank = rank;
    }

    public ConsoleColor GetConsoleColor()
    {
        switch (Suit)
        {
            case SUIT.Klöver:
                return ConsoleColor.DarkGreen;
            case SUIT.Spader:
                return ConsoleColor.DarkGray;
            case SUIT.Ruter:
                return ConsoleColor.DarkYellow;
            case SUIT.Hjärter:
                return ConsoleColor.DarkRed;
            default:
                return ConsoleColor.Black;
        }
    }
}

public enum SUIT
{
    Klöver,
    Ruter,
    Hjärter,
    Spader,
}

public enum RANK
{
    Two = 2,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine,
    Ten,
    Jack,
    Queen,
    King,
    Ace,
}