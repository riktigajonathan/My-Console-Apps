using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snake_test;

public class Death : Gamestate
{
    public Death() : base("death") { }

    public override void OnEnter()
    {
        Settings.gameLoopActive = false;
    }
}
