using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Retail_Tycoon
{
    internal struct Tile
    {
        public Vector2 Pos = Vector2.Zero;
        public TileVisual Visual = new();

        public Tile(Vector2 pos, TileVisual visual)
        {
            Pos = pos;

            Visual = visual;
        }

        public Tile(Vector2 pos, char charVisual = '?', ConsoleColor foregroundColor = ConsoleColor.White, ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            Pos = pos;

            Visual = new(charVisual, foregroundColor, backgroundColor);
        }

        public void Draw()
        {
            Screen.Tiles.Add(this);
        }
    }

    struct TileVisual
    {
        public char CharVisual;
        public ConsoleColor ForegroundColor;
        public ConsoleColor BackgroundColor;

        public TileVisual(char charVisual = '?', ConsoleColor foregroundColor = ConsoleColor.White, ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            CharVisual = charVisual;
            ForegroundColor = foregroundColor;
            BackgroundColor = backgroundColor;
        }
    }
}
