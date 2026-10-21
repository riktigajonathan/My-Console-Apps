using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Numerics;
using System.Text;

namespace newton_physics_test;

internal abstract class Shape
{
    public bool Fill = false;
    public float Rotation { get; set; } = 0.0f;
    public float Thickness { get; set; } = 1.0f;
    protected Vector2 position;

    public virtual void Draw(Canvas canvas)
    {
        // do nothing
    }

    protected static void SwapXY(ref Vector2 v)
    {
        float temp = v.X;
        v.X = v.Y;
        v.Y = temp;
    }

    public Vector2 GetPos()
    {
        Vector2 pos = position;

        SwapXY(ref pos);
        return pos;
    }
    public void SetPos(Vector2 pos)
    {
        SwapXY(ref pos);
        position = pos;
    }
}

internal class Circle : Shape
{
    public float Radius { get; set; }

    public Circle(float radius, float thickness = 1, float rotation = 0)
    {
        Radius = radius;
        Thickness = thickness;
        Rotation = rotation;
    }

    public override void Draw(Canvas canvas)
    {
        if (Fill)
        {
            canvas.DrawCircleFill(position.X, position.Y, Radius, Thickness);
        }
        else
        {
            canvas.DrawCircle(position.X, position.Y, Radius, Thickness);
        }
    }
}

internal class Rectangle : Shape
{
    public float Width { get; set; }
    public float Height { get; set; }

    public Rectangle(float width, float height, float thickness = 1, float rotation = 0)
    {
        Width = width;
        Height = height;
        Thickness = thickness;
        Rotation = rotation;
    }

    public override void Draw(Canvas canvas)
    {
        if (Fill)
        {
            canvas.DrawRectangleFill(position.X, position.Y, Width, Height, Rotation, Thickness);
        }
        else
        {
            canvas.DrawRectangle(position.X, position.Y, Width, Height, Rotation, Thickness);
        }
    }
}