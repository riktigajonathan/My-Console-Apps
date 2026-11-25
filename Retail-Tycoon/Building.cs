using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Retail_Tycoon
{
    internal class Building
    {
        public List<Floor> Floors = new();

        public Building(Vector2 size, int floorCount = 1)
        {
            for (int i = 0; i < floorCount; i++)
            {
                Floors.Add(new Floor(size));
            }
        }
    }
}
