namespace App._07
{
    internal class Run : IDay
    {
        public void Part1(IEnumerable<string> input)
        {
            string[] lines = [.. input];

            Dictionary<int, HashSet<int>> beams = [];
            int splits = 0;
            int beamStart = lines[0].IndexOf('S');
            beams[0] = [beamStart];

            for (int i = 1; i < lines.Length; i++)
            {
                HashSet<int> newBeams = [];
                for (int j = 0; j < lines[i].Length; j++)
                {
                    if (beams[i - 1].Contains(j) && lines[i][j] == '^')
                    {
                        newBeams.Add(j - 1);
                        newBeams.Add(j + 1);
                        splits++;
                    }
                    else if (beams[i - 1].Contains(j))
                        newBeams.Add(j);
                }

                beams[i] = newBeams;

            }

            Console.WriteLine(splits);
        }

        public void Part2(IEnumerable<string> input)
        {
            string[] lines = [.. input];

            long[] timelines = new long[lines[0].Length];

            for (int i = 0; i < lines.Length; i++)
            {
                Dictionary<int, long> previous = [];
                for (int t = 0; t < timelines.Length; t++)
                {
                    previous[t] = timelines[t];
                }

                for (int j = 0; j < lines[i].Length; j++)
                {
                    char c = lines[i][j];
                    if (c == 'S')
                        timelines[j] = 1;
                    else if (c == '^')
                    {
                        if (j <= 1)
                            timelines[j - 1] += previous.TryGetValue(j, out long prevCurr) ? prevCurr : timelines[j];
                        else
                            timelines[j - 1] += previous.TryGetValue(j, out long prevCurr) ? prevCurr : timelines[j]
                            + (previous.TryGetValue(j - 2, out long prevSecondLeft) ? prevSecondLeft : timelines[j - 2]);

                        if (j >= timelines.Length - 2)
                            timelines[j + 1] += previous.TryGetValue(j, out long prevCurr) ? prevCurr : timelines[j];
                        else
                            timelines[j + 1] += previous.TryGetValue(j, out long prevCurr) ? prevCurr : timelines[j]
                            + (previous.TryGetValue(j + 2, out long prevSecondRight) ? prevSecondRight : timelines[j + 2]);

                        timelines[j] = 0;
                    }
                }
            }

            Console.WriteLine(timelines.Sum());
        }

        public void Reset()
        {
            
        }
    }
}
