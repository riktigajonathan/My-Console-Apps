using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace Retail_Tycoon
{
    internal class Object
    {
        public Vector2 Position { get; set; }
        /// <summary>
        /// The building in which the person currently is inside of
        /// </summary>
        public Building? CurrentBuilding = null;

        public void EnterBuilding(Building building)
        {
            building.AddObject(this);
            CurrentBuilding = building;
        }

        public void ExitBuilding()
        {
            if (CurrentBuilding == null) return;

            CurrentBuilding.RemoveObject(this);
            CurrentBuilding = null;
        }
    }
}
