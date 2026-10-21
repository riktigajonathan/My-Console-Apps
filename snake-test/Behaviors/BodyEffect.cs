using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snake_test;

internal class BodyEffect
{
    public Action<List<Body>> action;

    public BodyEffect(Action<List<Body>> action) 
    {
        this.action = action;
    }

    public static Action<List<Body>> continuity = (bodies) =>
    {
        Reset(bodies);

        for (int i = 0; i < bodies.Count-1; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                Body body = bodies[i+j];
                List<Tile> shape = body.GetShape();
                Vector2i size = body.GetSize();
                Vector2i middlePos = Vector2i.Divide(size, new Vector2i(2, 2));
                Vector2i change;
                if (j == 0)
                {
                    change = Vector2i.Sub(bodies[i + 1].GetPos(), body.GetPos());
                }
                else
                {
                    change = Vector2i.Sub(bodies[i].GetPos(), body.GetPos());
                }
                
                change = Vector2i.Sign(change);
                if (Equals(change, Vector2i.ZERO)) change = Settings.startDir;
                if (Equals(change, Vector2i.DOWN))
                {
                    RemoveAt(new Vector2i(middlePos.x, size.y - 1), shape);
                    if ((j == 0 && i == 0) || (j == 1 && i == bodies.Count - 2))
                    {
                        RemoveAt(new Vector2i(size.x - 1, 0), shape);
                        RemoveAt(new Vector2i(0, 0), shape);
                    }
                }
                else if (Equals(change, Vector2i.UP))
                {
                    RemoveAt(new Vector2i(middlePos.x, 0), shape);
                    if ((j == 0 && i == 0) || (j == 1 && i == bodies.Count - 2))
                    {
                        RemoveAt(new Vector2i(size.x - 1, size.y - 1), shape);
                        RemoveAt(new Vector2i(0, size.y - 1), shape);
                    }
                }
                else if (Equals(change, Vector2i.RIGHT))
                {
                    RemoveAt(new Vector2i(size.x - 1, middlePos.y), shape);
                    if ((j == 0 && i == 0) || (j == 1 && i == bodies.Count - 2))
                    {
                        RemoveAt(new Vector2i(0, 0), shape);
                        RemoveAt(new Vector2i(0, size.y - 1), shape);
                    }
                }
                else if (Equals(change, Vector2i.LEFT))
                {
                    RemoveAt(new Vector2i(0, middlePos.y), shape);
                    if ((j == 0 && i == 0) || (j == 1 && i == bodies.Count - 2))
                    {
                        RemoveAt(new Vector2i(size.x - 1, 0), shape);
                        RemoveAt(new Vector2i(size.x - 1, size.y - 1), shape);
                    }
                }
            }
        }

        void RemoveAt(Vector2i pos, List<Tile> shape)
        {
            for (int i = 0; i < shape.Count; i++)
            {
                if (shape[i] == null) continue;

                if (Vector2i.Equals(shape[i].GetPos(), pos))
                {
                    shape[i].pixelValue = new PixelValue(ConsoleColor.Green, ConsoleColor.Black, 'o');
                }
            }
        }

        void Reset(List<Body> bodies)
        {
            foreach (Body body in bodies)
            {
                List<Tile> shape = body.GetShape();
                foreach (Tile t in shape)
                {
                    t.pixelValue = new PixelValue(ConsoleColor.Black, ConsoleColor.Green, '@');
                }
                Vector2i size = body.GetSize();
                Vector2i middlePos = Vector2i.Divide(size, new Vector2i(2, 2));
                RemoveAt(middlePos, shape);
            }
        }
    };

    public static Action<List<Body>> headify = (bodies) =>
    {
        Body body = bodies[0];
        List<Tile> shape = body.GetShape();
        Vector2i size = body.GetSize();
        Vector2i middlePos = Vector2i.Divide(size, new Vector2i(2, 2));

        void DrawAt(Vector2i pos, PixelValue pixelValue)
        {
            for (int i = 0; i < shape.Count; i++)
            {
                if (shape[i] == null) continue;

                if (Vector2i.Equals(shape[i].GetPos(), pos))
                {
                    shape[i].pixelValue = pixelValue;
                }
            }
        }
        Snake snake = bodies[0].parent;
        Vector2i dir = snake.lastDir;
        PixelValue eyeValue = new PixelValue(ConsoleColor.White, ConsoleColor.DarkGreen, 'O');
        PixelValue tongueValue;
        Vector2i pos;
        if (Vector2i.Equals(dir, Vector2i.DOWN))
        {
            tongueValue = new PixelValue(ConsoleColor.Red, ConsoleColor.Black, 'v');
            pos = new Vector2i(0, middlePos.y);
            DrawAt(pos, eyeValue);
            pos = new Vector2i(size.x - 1, middlePos.y);
            DrawAt(pos, eyeValue);
            foreach (Tile t in FrameBuffer.buffer)
            {
                if (t.pixelValue == tongueValue)
                {
                    FrameBuffer.buffer.Remove(t);
                    break;
                }
            }
            var nextPos = Vector2i.Add(bodies[0].GetPos(), Vector2i.Multiply(dir, new Vector2i(3, 3)));
            Tile tile = new Tile(new Vector2i(bodies[0].GetPos().x + middlePos.x, nextPos.y));
            FrameBuffer.Add(tile);
            tile.pixelValue = tongueValue;
        }
        else if (Vector2i.Equals(dir, Vector2i.RIGHT))
        {
            tongueValue = new PixelValue(ConsoleColor.Red, ConsoleColor.Black, '>');
            pos = new Vector2i(middlePos.x, 0);
            DrawAt(pos, eyeValue);
            pos = new Vector2i(middlePos.x, size.y - 1);
            DrawAt(pos, eyeValue);
            foreach (Tile t in FrameBuffer.buffer)
            {
                if (t.pixelValue == tongueValue)
                {
                    FrameBuffer.buffer.Remove(t);
                    break;
                }
            }
            var nextPos = Vector2i.Add(bodies[0].GetPos(), Vector2i.Multiply(dir, new Vector2i(3, 3)));
            Tile tile = new Tile(new Vector2i(nextPos.x, bodies[0].GetPos().y + middlePos.y));
            FrameBuffer.Add(tile);
            tile.pixelValue = tongueValue;
        }
        else if (Vector2i.Equals(dir, Vector2i.LEFT))
        {
            tongueValue = new PixelValue(ConsoleColor.Red, ConsoleColor.Black, '<');
            pos = new Vector2i(middlePos.x, 0);
            DrawAt(pos, eyeValue);
            pos = new Vector2i(middlePos.x, size.y - 1);
            DrawAt(pos, eyeValue);
            foreach (Tile t in FrameBuffer.buffer)
            {
                if (t.pixelValue == tongueValue)
                {
                    FrameBuffer.buffer.Remove(t);
                    break;
                }
            }
            var nextPos = Vector2i.Add(bodies[0].GetPos(), Vector2i.Multiply(dir, new Vector2i(3, 3)));
            Tile tile = new Tile(new Vector2i(bodies[0].GetPos().x - middlePos.x, nextPos.y+1));
            FrameBuffer.Add(tile);
            tile.pixelValue = tongueValue;
        }
        else if (Vector2i.Equals(dir, Vector2i.UP))
        {
            tongueValue = new PixelValue(ConsoleColor.Red, ConsoleColor.Black, '^');
            pos = new Vector2i(0, middlePos.y);
            DrawAt(pos, eyeValue);
            pos = new Vector2i(size.x - 1, middlePos.y);
            DrawAt(pos, eyeValue);
            foreach (Tile t in FrameBuffer.buffer)
            {
                if (t.pixelValue == tongueValue)
                {
                    FrameBuffer.buffer.Remove(t);
                    break;
                }
            }
            var nextPos = Vector2i.Add(bodies[0].GetPos(), Vector2i.Multiply(dir, new Vector2i(3, 3)));
            Tile tile = new Tile(new Vector2i(bodies[0].GetPos().x + middlePos.x, nextPos.y+2));
            FrameBuffer.Add(tile);
            tile.pixelValue = tongueValue;
        }
    };

    public static Action<List<Body>> solidify = (bodies) =>
    {
        foreach (Body body in bodies)
        {
            List<Tile> shape = body.GetShape();
            foreach (Tile t in shape)
            {
                t.pixelValue = new PixelValue(ConsoleColor.Black, ConsoleColor.Green, '@');
            }
        }
    }; 

    public static Action<List<Body>> drugged = (bodies) =>
    {
        Random rand = new Random();
        foreach (Body body in bodies)
        {
            List<Tile> shape = body.GetShape();
            foreach (Tile t in shape)
            {
                t.pixelValue = new PixelValue((ConsoleColor)rand.Next(0, 16), ConsoleColor.Black, t.pixelValue.character);
            }
        }
    };
}
