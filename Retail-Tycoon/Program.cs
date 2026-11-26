using System.Numerics;

namespace Retail_Tycoon
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Screen.Init();
            
            Boss player = new Boss();

            var store = new Store(new Vector2(10,2));
            player.OwnedBuildings.Add(store);

            player.EnterBuilding(store);

            while (true)
            {
                player.CurrentFloor.Draw();
                player.Draw();

                Screen.Flush();
            }
        }
    }
}
