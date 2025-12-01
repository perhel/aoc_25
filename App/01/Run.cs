namespace App._01
{
    internal class Run : IDay
    {
        private readonly int _min = 0;
        private readonly int _max = 99;
        private int _currentPos = 50;
        private int _atZeroCount = 0;

        public void Part1(IEnumerable<string> input)
        {
            IEnumerable<int> lines = input
                .Select(l => l.StartsWith('R') ? int.Parse(l[1..]) : -int.Parse(l[1..]));

            foreach (var n in lines)
            {
                int internalPos = _currentPos;
                for (int i = 0; i < Math.Abs(n); i++)
                {
                    if (n < 0) internalPos -= 1;
                    else internalPos += 1;

                    if (internalPos < 0) internalPos = 99;
                    if (internalPos > 99) internalPos = 0;
                }

                _currentPos = internalPos;
                if (_currentPos == 0) _atZeroCount++;
            }

            Console.WriteLine(_atZeroCount);
        }

        public void Part2(IEnumerable<string> input)
        {
            IEnumerable<int> lines = input
                .Select(l => l.StartsWith('R') ? int.Parse(l[1..]) : -int.Parse(l[1..]));

            foreach (var n in lines)
            {
                int internalPos = _currentPos;
                for (int i = 0; i < Math.Abs(n); i++)
                {
                    if (n < 0) internalPos -= 1;
                    else internalPos += 1;

                    if (internalPos < 0) internalPos = 99;
                    if (internalPos > 99) internalPos = 0;

                    if (internalPos == 0) _atZeroCount++;
                }

                _currentPos = internalPos;
            }

            Console.WriteLine(_atZeroCount);
        }

        public void Reset()
        {
            _atZeroCount = 0;
            _currentPos = 50;
        }
    }
}
