using System.Numerics;
using System.Runtime.Intrinsics;
using System.Threading;
using static System.Net.WebRequestMethods;

namespace snake_test;

internal class Program
{
    static void Main(string[] args)
    {
        FConsole.Initialize("snake");
        GamestateManager.InitStates();

        while (Settings.gameLoopActive)
        {
            GamestateManager.Update();
        }
    }
}