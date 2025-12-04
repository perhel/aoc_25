
namespace App._04
{
    internal class Run : IDay
    {
        private readonly HashSet<LongCoord> _rolls = [];
        private readonly HashSet<LongCoord> _pickable = [];

        public void Part1(IEnumerable<string> input)
        {
            string[] lines = [.. input];
            FindRolls(lines);
            foreach (LongCoord roll in _rolls.Where(r => LessThanXNeighbors(r, _rolls, 3)))
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
                IEnumerable<LongCoord> canPick = _rolls.Where(r => LessThanXNeighbors(r, _rolls, 3));

                if (!canPick.Any()) break;

                foreach (LongCoord roll in canPick)
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
                        _rolls.Add(LongCoord.FromXY(j, i));
                    }
                }
            }
        }

        private static bool LessThanXNeighbors(LongCoord roll, HashSet<LongCoord> rolls, int maxNeighbors)
        {
            int neighborCount = 0;

            for (int dX = -1; dX <= 1; dX++)
            {
                for (int dY = -1; dY <= 1; dY++)
                {
                    if (dX == 0 && dY == 0) continue;

                    if (rolls.Contains(LongCoord.FromXY(roll.X + dX, roll.Y + dY)))
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
