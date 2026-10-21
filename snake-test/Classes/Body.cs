using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snake_test;

internal class Body
{
    Vector2i pos;
    Vector2i size;
    public Snake parent;

    List<Tile> shape = [
        new Tile(new Vector2i(0,0)), new Tile(new Vector2i(1, 0)), new Tile(new Vector2i(2, 0)),
        new Tile(new Vector2i(0,1)), new Tile(new Vector2i(1, 1)), new Tile(new Vector2i(2, 1)),
        new Tile(new Vector2i(0,2)), new Tile(new Vector2i(1, 2)), new Tile(new Vector2i(2, 2)),
    ];

    public Body(Vector2i pos)
    {
        this.pos = pos;
        this.size = CalculateSize();
    }

    Vector2i CalculateSize()
    {
        if (shape.Count == 0) return new Vector2i(0, 0);

        int minX = int.MaxValue, maxX = int.MinValue;
        int minY = int.MaxValue, maxY = int.MinValue;

        foreach (Tile tile in shape)
        {
            var pos = tile.GetPos();
            if (pos.x < minX) minX = pos.x;
            if (pos.x > maxX) maxX = pos.x;
            if (pos.y < minY) minY = pos.y;
            if (pos.y > maxY) maxY = pos.y;
        }

        return new Vector2i((maxX - minX) + 1, (maxY - minY) + 1);
    }


    public Vector2i GetPos()
    {
        return pos;
    }

    public void SetPos(Vector2i newPos)
    {
        pos = new Vector2i(newPos.x, newPos.y);
    }

    public Vector2i GetSize()
    {
        return size;
    }

    public List<Tile> GetShape()
    {
        return shape;
    }

    public void SetShape(List<Tile> newShape)
    {
        shape = newShape;
        size = CalculateSize();
    }
}
