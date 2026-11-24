using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Retail_Tycoon
{
    internal class Floor
    {
        public List<Object> Objects = new();
        public string Display = "";

        public char VerticalWall = '|';
        public char HorizontalWall = '-';
        public char Corner = 'x';

        public Floor(Vector2 size)
        {
            for (int i = 0; i < size.X; i++)
            {
                for (int j = 0; j < size.Y; j++)
                {
                    char c = ' ';

                    bool isYEdge = j == 0 || j == size.Y - 1;
                    bool isXEdge = i == 0 || i == size.X - 1;

                    if (isYEdge && isXEdge)
                        c = Corner;
                    else if (isYEdge)
                        c = VerticalWall;
                    else if (isXEdge)
                        c = HorizontalWall;

                    Display += c + " ";
                }
                Display += '\n';
            }

            Console.WriteLine(Display);
        }
    }
}
