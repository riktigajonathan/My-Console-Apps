using System.Diagnostics;
using System.Text;

namespace FizzBuzz
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const int count = 100000;
            const int stopAtMatches = -1;

            KeyValuePair<int, string>[] rules = new KeyValuePair<int, string>[]
            {
                new(3, "Fizz"),
                new(5, "Buzz"),
                new(7, "Fuzz"),
                new(11, "Bizz"),
                new(13, "Biff")
            };

            StringBuilder sb = new();

            Stopwatch stopwatch = new();
            stopwatch.Start();

            for (int i = 1; i <= count; i++)
            {
                int matches = 0;

                for (int j = 0; j < rules.Length; j++)
                {
                    if (i % rules[j].Key == 0)
                    {
                        matches++;
                        sb.Append(rules[j].Value);
                    }
                }

                if (matches == 0)
                {
                    sb.Append(i);
                }
                else if (matches == stopAtMatches)
                {
                    sb.AppendLine();
                    sb.Append(stopAtMatches).Append(" match(es) found at number ").Append(i).AppendLine("!");
                    break;
                }

                sb.AppendLine();
            }

            stopwatch.Stop();
            long ticks = stopwatch.ElapsedTicks;
            double elapsedMs = (double)ticks / Stopwatch.Frequency * 1000;

            sb.Append('(').Append(elapsedMs.ToString("0.####")).Append(" ms elapsed)");

            Console.WriteLine(sb.ToString());
            Console.ReadKey(true);
        }
    }
}