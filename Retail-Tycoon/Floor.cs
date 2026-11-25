using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Retail_Tycoon
{
    internal class Floor
    {
        public List<Object> Objects = new();
        public List<Tile> Tiles = new();

        public TileVisual VerticalWall = new('|');
        public TileVisual HorizontalWall = new('-');
        public TileVisual Corner = new('x');
        public int WallThickness = 1;

        public Floor(Vector2 size)
        {
            size.X += WallThickness;
            size.Y += WallThickness;

            for (int i = 0; i < size.X; i++)
            {
                for (int j = 0; j < size.Y; j++)
                {
                    TileVisual visual = new();

                    bool isYEdge = j == 0 || j == size.Y - 1;
                    bool isXEdge = i == 0 || i == size.X - 1;

                    if (isYEdge && isXEdge)
                        visual = Corner;
                    else if (isYEdge)
                        visual = HorizontalWall;
                    else if (isXEdge)
                        visual = VerticalWall;

                    var pos = new Vector2(i, j);

                    Tiles.Add(new Tile(pos, visual));
                }
            }
        }
        public void AddObject(Object obj)
        {
            Objects.Add(obj);
        }

        public void RemoveObject(Object obj)
        {
            Objects.Remove(obj);
        }

        public void Draw()
        {
            foreach (Tile tile in Tiles)
            {
                tile.Draw();
            }

            foreach (Object obj in Objects)
            {
                obj.Draw();
            }
        }
    }
}
