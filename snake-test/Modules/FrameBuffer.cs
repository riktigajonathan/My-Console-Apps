using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snake_test;

internal class FrameBuffer
{
    public static List<Tile> buffer = new List<Tile>();

    public static void Clear()
    {
        buffer.Clear();
    }

    public static void Add(Tile tile)
    {
        buffer.Add(tile);
    }

    public static void Draw()
    {
        List<Vector2i> seen = new List<Vector2i>();
        
        for (int i = buffer.Count-1; i >= 0; i--)
        {

            var pos = buffer[i].GetPos();
            if (!seen.Contains(pos))
            {
                seen.Add(pos);
            }
            else
            {
                buffer.RemoveAt(i);
            }
        }

        for (int i = 0; i < buffer.Count; i++)
        {
            Tile tile = buffer[i];
            var pos = tile.GetPos();
            int xScale = 2;
            pos.x *= xScale;

            int bufferWidth = Console.BufferWidth;
            int bufferHeight = Console.BufferHeight;

            if (bufferWidth <= 0 || bufferHeight <= 0)
                return;

            if (pos.x < 0 || pos.x >= bufferWidth ||
                pos.y < 0 || pos.y >= bufferHeight ||
                pos.x+1 > Settings.mapWidth * xScale || 
                pos.y+1 > Settings.mapHeight)
            {
                continue;
            }

            FConsole.SetChar((short)pos.x, (short)pos.y, tile.pixelValue);
        }

        FConsole.DrawBuffer();
    }

    public static Tile? TileAt(Vector2i pos)
    {
        foreach (Tile t in buffer)
        {
            if (Vector2i.Equals(t.GetPos(), pos)) return t;
        }

        return null;
    }
}
