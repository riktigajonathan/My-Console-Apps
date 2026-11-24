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

                    if (j == 0 || j == size.Y - 1)
                    {
                        c = VerticalWall;
                    }
                    if (i == 0 || i == size.X-1)
                    {
                        if (c == VerticalWall)
                        {
                            c = Corner;
                        }
                        else
                        {
                            c = HorizontalWall;
                        }
                    }

                    Display += c + " ";
                }
                Display += '\n';
            }

            Console.WriteLine(Display);
        }
    }
}
