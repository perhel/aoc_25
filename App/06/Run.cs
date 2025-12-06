using System.Collections.Concurrent;
using System.Runtime.InteropServices;

namespace App._06
{
    internal enum Op
    {
        Add = 0,
        Multiply = 9000
    }

    internal class Run : IDay
    {
        private readonly Dictionary<int, long> _results = [];

        public void Part1(IEnumerable<string> input)
        {
            string[] lines = [.. input];

            Dictionary<int, char> ops = lines[^1]
                .Select(c => c)
                .Where(c => c != ' ')
                .Select((c, i) => (i, c))
                .ToDictionary();

            for (int i = 0; i < ops.Count; i++)
            {
                _results.Add(i, 0);
            }

            for (int i = 0; i < lines.Length - 1; i++)
            {
                long[] values = [.. lines[i]
                    .Split(' ')
                    .Where(p => p.Length > 0)
                    .Select(long.Parse)];

                for (int v = 0; v < values.Length; v++)
                {
                    
                    if (ops[v] == '+') _results[v] += values[v];
                    if (ops[v] == '*')
                    {
                        if (_results[v] == 0) _results[v] = 1;
                        _results[v] *= values[v];
                    }
                }
            }

            Console.WriteLine(_results.Select(r => r.Value).Sum());
        }

        public void Part2(IEnumerable<string> input)
        {
            string[] lines = [.. input];

            // Skapa ny tom matris
            char[,] chars = new char[lines[0].Length, lines.Length];

            // Fyll matris med lines roterad moturs
            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = lines[i].Length - 1; j >= 0; j--)
                {
                    chars[chars.GetLength(0) - 1 - j, i] = lines[i][j];
                }
            }

            // Gå igenom de nya raderna och summera eller multiplicera när tecken dyker upp
            List<long> results = [];
            List<long> valuesCache = [];
            bool sum = false;
            bool multiply = false;
            for (int i = 0; i < chars.GetLength(0); i++)
            {
                long val = 0;
                for (int j = 0; j < chars.GetLength(1); j++)
                {
                    if (chars[i, j] == ' ') continue;
                    else if (chars[i, j] == '+') sum = true;
                    else if (chars[i, j] == '*') multiply = true;

                    if (sum || multiply) break;

                    val = val * (10 * (val == 0 ? 0 : 1)) + (chars[i, j] - '0');
                }

                if (val > 0) valuesCache.Add(val);

                if (sum)
                    results.Add(valuesCache.Aggregate((long)0, (acc, n) => acc += n));

                if (multiply)
                    results.Add(valuesCache.Aggregate((long)1, (acc, n) => acc *= n));

                if (sum || multiply)
                {
                    sum = false;
                    multiply = false;
                    valuesCache = [];
                }
            }

            Console.WriteLine(results.Sum());
        }

        public void Reset()
        {
            
        }
    }
}
