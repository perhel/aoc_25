namespace App._05
{
    internal class Run : IDay
    {
        private readonly HashSet<(long start, long end)> _freshRanges = [];
        private readonly List<long> _fresh = [];

        public void Part1(IEnumerable<string> input)
        {
            string[] lines = [.. input];
            int i = 0;

            while (true)
            {
                string line = lines[i];
                i++;
                
                if (string.IsNullOrEmpty(line)) break;

                var range = line.Split('-');
                var start = long.Parse(range[0]);
                var end = long.Parse(range[1]);

                _freshRanges.Add((start, end));

            }

            for (int j = i; i < lines.Length; i++)
            {
                var val = long.Parse(lines[i]);

                if (_freshRanges.Any(r => val >= r.start && val <= r.end))
                {
                    _fresh.Add(val);
                }
            }

            Console.WriteLine(_fresh.Count);
        }

        public void Part2(IEnumerable<string> input)
        {
            HashSet<(long start, long end)> idRanges = [];
            (long, long) notFound = (0, 0);
            foreach ((long start, long end) range in _freshRanges)
            {
                if (idRanges.Count == 0)
                {
                    idRanges.Add(range);
                    continue;
                }

                (long, long) contained = idRanges
                    .Where(r => r.start <= range.start && r.end >= range.end)
                    .FirstOrDefault(notFound);

                if (contained != notFound) continue;

                (long, long) startInside = idRanges
                    .Where(r => r.start <= range.start && r.end >= range.start)
                    .FirstOrDefault(notFound);

                (long, long) endInside = idRanges
                    .Where(r => r.start <= range.end && r.end >= range.end)
                    .FirstOrDefault(notFound);

                (long, long) newRange = (0, 0);

                if (startInside != notFound && endInside == notFound)
                    newRange = (startInside.Item1, range.end);
                else if (startInside == notFound && endInside != notFound)
                    newRange = (range.start, endInside.Item2);
                else if (startInside != notFound && endInside != notFound)
                    newRange = (startInside.Item1, endInside.Item2);
                else
                    newRange = range;

                idRanges.RemoveWhere(r => r.start >= newRange.Item1 && r.end <= newRange.Item2);

                idRanges.Add(newRange);
            }

            long idCount = idRanges.Aggregate((long)0, (acc, n) => acc += 1 + n.end - n.start);

            Console.WriteLine(idCount);
        }

        public void Reset()
        {
            
        }
    }
}
