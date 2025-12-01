using System.Diagnostics;

namespace App
{
    internal static class Util
    {
        public static void RunDaily(bool test = false, int? dayOverride = null)
        {
            try
            {
                string date = (dayOverride ?? DateTimeOffset.Now.Day).ToString().PadLeft(2, '0');
                Type? type = System.Reflection.Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .FirstOrDefault(t => t.FullName == $"App._{date}.Run");
    
                if (type is not null && type.IsAssignableTo(typeof(IDay)))
                {
                    Stopwatch sw = Stopwatch.StartNew();
                    IDay day = (IDay)Activator.CreateInstance(type)!;
                    Console.WriteLine($"--- Day{date} Part 1 ---");
                    day.Part1(ReadInputFleLines(Path.Combine(date, test ? "test.txt" : "input.txt")));
                    Console.WriteLine($"Completed in {sw.ElapsedMilliseconds} ms");
                    sw.Restart();
                    day.Reset();
                    Console.WriteLine($"--- Day{date} Part 2 ---");
                    day.Part2(ReadInputFleLines(Path.Combine(date, test ? "test.txt" : "input.txt")));
                    Console.WriteLine($"Completed in {sw.ElapsedMilliseconds} ms");
                    Console.WriteLine();
                    sw.Stop();
                }
                else Console.WriteLine($"Day {date} is not implemented correctly...");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static string[] ReadInputFleLines(string path)
            => File.ReadAllLines(path);
    }
}
