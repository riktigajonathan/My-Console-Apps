using System.Numerics;
using System.Runtime.Intrinsics;
using static System.Net.WebRequestMethods;
using System.Threading;

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