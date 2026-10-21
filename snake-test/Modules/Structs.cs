using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace snake_test;

public struct Vector2i
{
    public static readonly Vector2i ONE = new Vector2i(1, 1);
    public static readonly Vector2i ZERO = new Vector2i(0, 0);
    public static readonly Vector2i UP = new Vector2i(0, -1);
    public static readonly Vector2i DOWN = new Vector2i(0, 1);
    public static readonly Vector2i LEFT = new Vector2i(-1, 0);
    public static readonly Vector2i RIGHT = new Vector2i(1, 0);

    public int x, y;

    public Vector2i(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public static Vector2i Add(Vector2i left, Vector2i right)
    {
        return new Vector2i(left.x + right.x, left.y + right.y);
    }

    public static Vector2i Sub(Vector2i left, Vector2i right)
    {
        return new Vector2i(left.x - right.x, left.y - right.y);
    }

    public static Vector2i Multiply(Vector2i left, Vector2i right)
    {
        return new Vector2i(left.x * right.x, left.y * right.y);
    }

    public static Vector2i Divide(Vector2i left, Vector2i right)
    {
        return new Vector2i((int)((float)left.x / (float)right.x), (int)((float)left.y / (float)right.y));
    }

    public static bool Equals(Vector2i left, Vector2i right)
    {
        return left.x == right.x && left.y == right.y;
    }

    public static Vector2i Invert(Vector2i vec)
    {
        return new Vector2i(vec.x * -1, vec.y * -1);
    }

    public static Vector2i Sign(Vector2i vec)
    {
        return new Vector2i(Math.Sign(vec.x), Math.Sign(vec.y));
    }
}

public abstract class Gamestate 
{
    public string name { get; }

    protected Gamestate(string name)
    {
        this.name = name;
    }

    public virtual void OnEnter() { }
    public virtual void OnExit() { }
    public virtual void Update() { }
}