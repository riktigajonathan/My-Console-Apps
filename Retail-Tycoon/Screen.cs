using System;
using System.Collections.Generic;
using System.Text;

namespace Retail_Tycoon
{
    internal static class Screen
    {
        public static List<Tile> Tiles = new();
        public static ConsoleColor BackgroundColor = ConsoleColor.Black;

        public static void Init()
        {
            Tiles = new();
            Console.CursorVisible = false;
            Console.Clear();

            for (int i = 0; i < Console.WindowWidth; i++)
            {
                for (int j = 0; j < Console.WindowHeight; j++)
                {
                    Console.BackgroundColor = BackgroundColor;
                    Console.SetCursorPosition(i,j);
                    Console.Write(" ");
                }
            }
        }

        public static void Flush()
        {
            foreach (var tile in Tiles)
            {
                Console.SetCursorPosition((int)(tile.Pos.X*2), (int)tile.Pos.Y);
                Console.ForegroundColor = tile.Visual.ForegroundColor;
                Console.BackgroundColor = tile.Visual.BackgroundColor;
                Console.Write(tile.Visual.CharVisual);
            }

            Tiles.Clear();
        }
    }
}
