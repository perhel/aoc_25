namespace App._02
{
    internal class Run : IDay
    {
        private List<long> _invalidIds = [];
        private HashSet<long> _cache = [];

        public void Part1(IEnumerable<string> input)
        {
            _invalidIds = [.. input.Select(l => l.Split(','))
                .SelectMany(x => x.Select(y => IdRange.FromString(y)))
                .Select(r => r.Validate1(_cache))
                .SelectMany(a => a)];

            Console.WriteLine(_invalidIds.Sum());
        }

        public void Part2(IEnumerable<string> input)
        {
            _invalidIds = [.. input.Select(l => l.Split(','))
                .SelectMany(x => x.Select(y => IdRange.FromString(y)))
                .Select(r => r.Validate2(_cache))
                .SelectMany(a => a)];

            Console.WriteLine(_invalidIds.Sum());
        }

        public void Reset()
        {
            _invalidIds = [];
            _cache = [];
        }
    }

    internal class IdRange
    {
        public long Start { get; set; }
        public long End { get; set; }

        public List<long> InvalidIds { get; set; } = [];

        public static IdRange FromString(string range)
        {
            string[] parts = range.Split('-');
            long start = long.Parse(parts[0]);
            long end = long.Parse(parts[1]);

            return new() { Start = start, End = end };
        }

        public List<long> Validate1(HashSet<long> cache)
        {
            for (long i = Start; i <= End; i++)
            {
                if (cache.Contains(i))
                {
                    InvalidIds.Add(i);
                    continue;
                }

                string s = i.ToString();
                int l = s.Length;

                if (l % 2 != 0) continue;

                string first = s[..(l / 2)];
                string second = s[(l / 2)..];

                if (first == second)
                {
                    InvalidIds.Add(i);
                    cache.Add(i);
                    continue;
                }

                if (s.Distinct().Count() == 1)
                {
                    InvalidIds.Add(i);
                    cache.Add(i);
                }
            }

            return InvalidIds;
        }

        public List<long> Validate2(HashSet<long> cache)
        {
            for (long i = Start; i <= End; i++)
            {
                if (cache.Contains(i))
                {
                    InvalidIds.Add(i);
                    continue;
                }

                string s = i.ToString();
                int l = s.Length;

                string first = s[..(l / 2)];
                string second = s[(l / 2)..];

                if (first == second)
                {
                    InvalidIds.Add(i);
                    cache.Add(i);
                    continue;
                }

                if (l > 1 && s.Distinct().Count() == 1)
                {
                    InvalidIds.Add(i);
                    cache.Add(i);
                    continue;
                }

                for (int j = 2; j < l / 2; j++)
                {
                    IEnumerable<long> chunks = s.Chunk(j)
                        .Select(ch =>
                        {
                            long res = 0;
                            for (int i = 0; i < ch.Length; i++)
                            {
                                char c = ch[i];
                                int digit = c - '0';
                                res = res * 10 + digit;
                            }
                            return res;
                        });

                    if (!chunks.Any()) continue;

                    long t = chunks.First();
                    if (chunks.Distinct().Count() == 1)
                    {
                        InvalidIds.Add(i);
                        cache.Add(i);
                        break;
                    }
                }
            }

            return InvalidIds;
        }
    }
}
