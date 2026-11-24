using System.Numerics;

namespace Retail_Tycoon
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Boss player = new Boss();

            var store = new Store(new Vector2(4,4));
            player.OwnedBuildings.Add(store);
            
            player.EnterBuilding(store);

            
        }
    }
}
