
namespace App._04
{
    internal class Run : IDay
    {
        private readonly HashSet<long> _rolls = [];
        private readonly HashSet<long> _pickable = [];

        public void Part1(IEnumerable<string> input)
        {
            string[] lines = [.. input];
            FindRolls(lines);
            foreach (long roll in _rolls.Where(r => LessThanXNeighbors(r, _rolls, 3)))
            {
                _pickable.Add(roll);
            }


            Console.WriteLine(_pickable.Count.ToString());
        }

        public void Part2(IEnumerable<string> input)
        {
            string[] lines = [.. input];
            FindRolls(lines);
            while (true)
            {
                IEnumerable<long> canPick = _rolls.Where(r => LessThanXNeighbors(r, _rolls, 3));

                if (!canPick.Any()) break;

                foreach (long roll in canPick)
                {
                    _pickable.Add(roll);
                    _rolls.Remove(roll);
                }
            }

            Console.WriteLine(_pickable.Count.ToString());
        }

        private void FindRolls(string[] lines)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    if (lines[i][j] == '@')
                    {
                        _rolls.Add(Encode(j, i));
                    }
                }
            }
        }

        private static long Encode(int x, int y)
            => ((long)x << 32) | (uint)y;

        private static int DecodeX(long coord)
            => (int)(coord >> 32);

        private static int DecodeY(long coord)
            => (int)coord;

        private static bool LessThanXNeighbors(long roll, HashSet<long> rolls, int maxNeighbors)
        {
            int rollX = DecodeX(roll);
            int rollY = DecodeY(roll);

            int neighborCount = 0;

            for (int dX = -1; dX <= 1; dX++)
            {
                for (int dY = -1; dY <= 1; dY++)
                {
                    if (dX == 0 && dY == 0) continue;

                    if (rolls.Contains(Encode(rollX + dX, rollY + dY)))
                        neighborCount++;

                    if (neighborCount > maxNeighbors) return false;
                }
            }

            return true;
        }

        public void Reset()
        {
            _rolls.Clear();
            _pickable.Clear();
        }
    }
}
