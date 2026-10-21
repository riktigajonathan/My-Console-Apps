using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snake_test;

public static class Settings
{
    public static int mapWidth = 48;
    public static int mapHeight = 24;

    public static int startLength = 7;
    public static Vector2i startPos = Vector2i.ZERO;
    public static Vector2i startDir = Vector2i.DOWN;

    public static int waitMs = 100;
    public static char[] livableTiles = { '.', 'v', '^', '>', '<' };
    public static bool gameLoopActive = true;
}