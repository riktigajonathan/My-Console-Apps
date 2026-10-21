using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snake_test;

internal class Player
{
    Snake snake;
    Vector2i dir = Settings.startDir;
    Dictionary<ConsoleKey, Action> keybinds = new();

    public Player(Vector2i startPos)
    {
        snake = new Snake(startPos, Settings.startLength);

        RebindToArrowKeys();

        snake.AddEffect(BodyEffect.continuity);
        snake.AddEffect(BodyEffect.headify);
    }

    public void RebindToWASD()
    {
        keybinds.Clear();
        keybinds.Add(ConsoleKey.W, () => dir = Vector2i.Equals(dir, Vector2i.DOWN) ? dir : Vector2i.UP);
        keybinds.Add(ConsoleKey.S, () => dir = Vector2i.Equals(dir, Vector2i.UP) ? dir : Vector2i.DOWN);
        keybinds.Add(ConsoleKey.D, () => dir = Vector2i.Equals(dir, Vector2i.LEFT) ? dir : Vector2i.RIGHT);
        keybinds.Add(ConsoleKey.A, () => dir = Vector2i.Equals(dir, Vector2i.RIGHT) ? dir : Vector2i.LEFT);
    }

    public void RebindToArrowKeys()
    {
        keybinds.Clear();
        keybinds.Add(ConsoleKey.UpArrow, () => dir = Vector2i.Equals(dir, Vector2i.DOWN) ? dir : Vector2i.UP);
        keybinds.Add(ConsoleKey.DownArrow, () => dir = Vector2i.Equals(dir, Vector2i.UP) ? dir : Vector2i.DOWN);
        keybinds.Add(ConsoleKey.RightArrow, () => dir = Vector2i.Equals(dir, Vector2i.LEFT) ? dir : Vector2i.RIGHT);
        keybinds.Add(ConsoleKey.LeftArrow, () => dir = Vector2i.Equals(dir, Vector2i.RIGHT) ? dir : Vector2i.LEFT);
    }

    public void Update()
    {
        InputCheck();
        Move();
        snake.Update();
    }

    public void InputCheck()
    {
        if (Console.KeyAvailable)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            foreach (ConsoleKey key in keybinds.Keys)
            {
                if (keyInfo.Key == key)
                {
                    keybinds[key].Invoke();
                }
            }
        }
    }

    public void Move()
    {
        snake.Move(dir);
    }
}
