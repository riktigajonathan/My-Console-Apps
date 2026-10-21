using System.Text;
using System.Numerics;
using System.Diagnostics;

namespace newton_physics_test;

internal class Program
{
    static void Main(string[] args)
    {
        Console.CursorVisible = false;
        Canvas canvas = new(103, 60);

        Body circle = new(new Vector2(20, 20), new Rectangle(10,10), true);

        Stopwatch stopwatch = Stopwatch.StartNew();
        long lastTicks = stopwatch.ElapsedTicks;

        const int targetFPS = 60;
        var frameTime = 1.0f / (float)targetFPS; // Target frame time for 60 FPS

        while (true)
        {
            float deltaTime = GetDeltaTime();

            // temp
            for (int i = 0; i < 103; i++)
            {
                canvas.pixels[50,i] = '█';
            }

            Body.UpdateAll(deltaTime);

            frameTime -= deltaTime;
            if (frameTime < 0)
            {
                Body.DrawAll(canvas);
                frameTime = 1.0f / (float)targetFPS; // Reset frame time for next frame
            }

            canvas.Flush();
        }

        float GetDeltaTime()
        {
            long currentTicks = stopwatch.ElapsedTicks;
            float deltaTime = (currentTicks - lastTicks) / (float)Stopwatch.Frequency;
            lastTicks = currentTicks;
            return deltaTime;
        }
    }
}
