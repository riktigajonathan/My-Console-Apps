using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snake_test;

public class Game : Gamestate
{
    public Game() : base("game") { }

    List<Player> players = new();
    Map map;

    public override void OnEnter()
    {
        players.Add(new(Settings.startPos));
        map = new();
    }

    public override void Update()
    {
        map.Update();
        foreach (Player player in players)
        {
            player.Update();
        }

        FrameBuffer.Draw();

        Thread.Sleep(Settings.waitMs);
    }
}
