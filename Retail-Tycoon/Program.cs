namespace Retail_Tycoon
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Boss player = new Boss();

            var store = new Store();
            player.OwnedBuildings.Add(store);
            
            player.EnterBuilding(store);

            player.CurrentBuilding.Draw();
        }
    }
}
