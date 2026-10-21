using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Pipelines;
using System.Numerics;
using System.Text;

namespace newton_physics_test;

internal class Canvas
{
    const string LightLevels = @"$@B%8&WM#*oahkbdpqwmZO0QLCJUYXzcvunxrjft/\|()1{}[]?-_+~<>i!lI;:,""^`'.";

    int width = 0;
    int height = 0;

    public char[,] pixels;

    public Canvas(int h, int w)
    {
        width = w;
        height = h;

        pixels = new char[w, h];
        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                pixels[i, j] = ' ';
            }
        }
    }

    public void Flush()
    {
        StringBuilder sb = new StringBuilder();
        char borderChar = '-';

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                sb.Append(pixels[i, j]);
                sb.Append(' ');
            }
            sb.Append(borderChar);
            sb.Append('\n');
        }
        for (int i = 0; i <= height; i++)
        {
            sb.Append(borderChar);
            sb.Append(' ');
        }

        Console.SetCursorPosition(0, 0);
        Console.WriteLine(sb.ToString());

        Clear();
    }

    public void Clear()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                pixels[i, j] = ' ';
            }
        }
    }

    public void DrawCircle(float x, float y, float r, float thickness = 1)
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                double d = Math.Abs(Math.Sqrt((i - x) * (i - x) + (j - y) * (j - y)) - r);

                if (d < thickness)
                {
                    double index = Math.Floor((double)LightLevels.Length * (d / thickness));

                    index -= LightLevels.IndexOf(pixels[i, j]);
                    index = Math.Min(index, LightLevels.Length - 1);
                    index = Math.Max(index, 0);

                    pixels[i, j] = LightLevels[(int)index];
                }
            }
        }
    }

    public void DrawCircleFill(float x, float y, float r, float thickness = 1)
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                float distFromCenter = MathF.Sqrt((i - x) * (i - x) + (j - y) * (j - y));
                float d = Math.Abs(distFromCenter - r);

                if (distFromCenter <= r)
                {
                    pixels[i, j] = LightLevels[0];
                }
                else if (d < thickness)
                {
                    double index = Math.Floor((double)LightLevels.Length * (d / thickness));
                    index = Math.Min(index, LightLevels.Length - 1);
                    index = Math.Max(index, 0);

                    pixels[i, j] = LightLevels[(int)index];
                }
            }
        }
    }

    public void DrawRectangle(float rx, float ry, float rw, float rh, float angle = 0, float thickness = 1)
    {
        float halfW = rw / 2f;
        float halfH = rh / 2f;
        float cx = rx + halfW;
        float cy = ry + halfH;
        float cos = MathF.Cos(-angle);
        float sin = MathF.Sin(-angle);

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                float dx = i - cx;
                float dy = j - cy;
                float localX = dx * cos - dy * sin;
                float localY = dx * sin + dy * cos;

                float qx = Math.Abs(localX) - halfW;
                float qy = Math.Abs(localY) - halfH;
                float outsideDist = MathF.Sqrt(Math.Max(qx, 0f) * Math.Max(qx, 0f) + Math.Max(qy, 0f) * Math.Max(qy, 0f));
                float insideDist = Math.Min(Math.Max(qx, qy), 0.0f);
                float sdf = outsideDist + insideDist;

                double d = Math.Abs(sdf);

                if (d < thickness)
                {
                    double index = Math.Floor((double)LightLevels.Length * (d / thickness));

                    index -= LightLevels.IndexOf(pixels[i, j]);
                    index = Math.Min(index, LightLevels.Length - 1);
                    index = Math.Max(index, 0);

                    pixels[i, j] = LightLevels[(int)index];
                }
            }
        }
    }

    public void DrawRectangleFill(float rx, float ry, float rw, float rh, float angle = 0, float thickness = 1)
    {
        float halfW = rw / 2f;
        float halfH = rh / 2f;
        float cx = rx + halfW;
        float cy = ry + halfH;
        float cos = MathF.Cos(-angle);
        float sin = MathF.Sin(-angle);

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                float dx = i - cx;
                float dy = j - cy;
                float localX = dx * cos - dy * sin;
                float localY = dx * sin + dy * cos;

                float qx = Math.Abs(localX) - halfW;
                float qy = Math.Abs(localY) - halfH;
                float outsideDist = MathF.Sqrt(Math.Max(qx, 0f) * Math.Max(qx, 0f) + Math.Max(qy, 0f) * Math.Max(qy, 0f));
                float insideDist = Math.Min(Math.Max(qx, qy), 0.0f);
                float sdf = outsideDist + insideDist;

                if (sdf <= 0)
                {
                    pixels[i, j] = LightLevels[0];
                }
                else if (sdf < thickness)
                {
                    double index = Math.Floor((double)LightLevels.Length * (sdf / thickness));
                    index = Math.Min(index, LightLevels.Length - 1);
                    index = Math.Max(index, 0);

                    pixels[i, j] = LightLevels[(int)index];
                }
            }
        }
    }
}