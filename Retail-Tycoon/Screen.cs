using System;
using System.Collections.Generic;
using System.Text;

namespace Retail_Tycoon
{
    internal static class Screen
    {
        public static List<Tile> Tiles = new();

        public static void Init()
        {
            Tiles = new();
            Console.CursorVisible = false;
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
