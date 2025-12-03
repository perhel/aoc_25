
namespace App._03
{
    internal class Run : IDay
    {
        public void Part1(IEnumerable<string> input)
        {
            int joltageNumberCount = 2;
            IEnumerable<long> batteries = input.Select(l => l.Trim())
                .Select(line => line.Select(c => c - '0').ToArray())
                .Select(digitArray => BuildJoltage(ref digitArray, joltageNumberCount));

            Console.WriteLine(batteries.Sum());
        }

        public void Part2(IEnumerable<string> input)
        {
            int joltageNumberCount = 12;
            IEnumerable<long> batteries = input.Select(l => l.Trim())
                .Select(line => line.Select(c => c - '0').ToArray())
                .Select(digitArray => BuildJoltage(ref digitArray, joltageNumberCount));

            Console.WriteLine(batteries.Sum());
        }

        private static long BuildJoltage(ref int[] digits, int joltageNumberCount)
        {
            long joltage = 0;
            for (int i = joltageNumberCount - 1; i >= 0; i--)
            {
                int indexOfMax = IndexOfMaxDigitWithAtleastNTrailing(digits, i);
                joltage = joltage * 10 + digits[indexOfMax];

                Span<int> trail = ((Span<int>)digits)[(indexOfMax + 1)..];
                digits = trail.ToArray();
            }
            return joltage;
        }

        private static int IndexOfMaxDigitWithAtleastNTrailing(int[] digits, int nTrail)
        {
            long max = -1;
            int indexOfMax = -1;
            for (int i = 0; i < digits.Length - nTrail; i++)
            {
                if (digits[i] > max)
                {
                    max = digits[i];
                    indexOfMax = i;
                }
            }
            return indexOfMax;
        }

        public void Reset() { }
    }
}
