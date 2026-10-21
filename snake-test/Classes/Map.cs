using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snake_test;

internal class Map
{
    public static List<Tile> tiles = new List<Tile>();

    public Map()
    {
        int width = Settings.mapWidth;
        int height = Settings.mapHeight;

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                PixelValue pixelValue = new PixelValue(ConsoleColor.Gray, ConsoleColor.Black, '.');
                Tile newTile = new Tile(new Vector2i(j, i), pixelValue);
                tiles.Add(newTile);
            }
        }
    }

    public void Update()
    {
        QueueDraw();
    }

    public void QueueDraw()
    {
        foreach (var tile in tiles)
        {
            FrameBuffer.Add(tile);
        }
    }
}
