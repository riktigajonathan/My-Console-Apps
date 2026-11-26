using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace Retail_Tycoon
{
    internal class Object
    {
        public Vector2 Pos = Vector2.Zero;
        public List<Tile> Tiles = new();
        /// <summary>
        /// The floor in which the object currently is inside of
        /// </summary>
        public Floor? CurrentFloor = null;

        public void EnterBuilding(Building building) => EnterFloor(building.Floors[0]);

        public void ExitBuilding(Building building) => ExitFloor();

        public void EnterFloor(Floor floor)
        {
            floor.AddObject(this);
            CurrentFloor = floor;
        }

        public void ExitFloor()
        {
            if (CurrentFloor == null) return;

            CurrentFloor.RemoveObject(this);
            CurrentFloor = null;
        }

        public void Draw()
        {
            foreach (var tile in Tiles) 
            {
                var drawTile = tile;
                drawTile.Pos += Pos;

                // adjust the posittion out of the floor's wall
                if (CurrentFloor != null)
                    drawTile.Pos += Vector2.One*CurrentFloor.WallThickness;

                Screen.Tiles.Add(drawTile);
            }
        }
    }
}
