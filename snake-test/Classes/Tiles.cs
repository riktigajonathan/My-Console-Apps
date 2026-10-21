using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snake_test;

internal class Tile
{
    Vector2i pos;
    public PixelValue pixelValue;

    public Tile(Vector2i pos, PixelValue? pixelvalue = null)
    {
        this.pos = pos;
        if (pixelvalue == null) 
        {
            pixelvalue = new PixelValue(ConsoleColor.White, ConsoleColor.Black, 'O');
        }
        this.pixelValue = (PixelValue)pixelvalue;
    }

    public Vector2i GetPos()
    {
        return pos;
    }

    static public List<Vector2i> ExtractPositions(List<Tile> tiles)
    {
        var toReturn = new List<Vector2i>();

        foreach (Tile tile in tiles)
        {
            toReturn.Add(tile.pos);
        }

        return toReturn;
    }
}
