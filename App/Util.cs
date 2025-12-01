namespace App
{
    internal static class Util
    {
        public static void RunDaily(bool test = false, int? day = null)
        {
            string date = (day ?? DateTimeOffset.Now.Day).ToString().PadLeft(2, '0');
            Type? type = System.Reflection.Assembly.GetExecutingAssembly()
                .GetTypes()
                .FirstOrDefault(t => t.FullName.Equals($"App._{date}.Run"));

            if (type is not null && type.IsAssignableTo(typeof(IDay)))
            {
                IDay day = (IDay)Activator.CreateInstance(type);
                day.Part1(ReadInputFleLines(Path.Combine(date, test ? "test.txt" : "input.txt")));
                day.Reset();
                day.Part2(ReadInputFleLines(Path.Combine(date, test ? "test.txt" : "input.txt")));
            }
            else Console.WriteLine($"Day {date} is not implemented...");
        }

        private static IEnumerable<string> ReadInputFleLines(string path)
            => File.ReadAllLines(path);
    }
}
