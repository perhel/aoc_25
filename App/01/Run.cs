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
            foreach (var line in input)
            {
                int move = ParseMove(line);
                _currentPos = (_currentPos + move) % 100;

                if (_currentPos == 0) _atZeroCount++;
            }

            Console.WriteLine(_atZeroCount);
        }

        public void Part2(IEnumerable<string> input)
        {
            foreach (var line in input)
            {
                int move = ParseMove(line);

                int internalPos = _currentPos;
                for (int i = 0; i < Math.Abs(move); i++)
                {
                    int step = move < 0 ? -1 : 1;
                    internalPos = (internalPos + step) % 100;

                    if (internalPos == 0) _atZeroCount++;
                }

                _currentPos = internalPos;
            }

            Console.WriteLine(_atZeroCount);
        }

        private static int ParseMove(string line)
        {
            int move = int.Parse(line[1..]);
            if (line.StartsWith('L')) move = -move;
            return move;
        }

        public void Reset()
        {
            _atZeroCount = 0;
            _currentPos = 50;
        }
    }
}
