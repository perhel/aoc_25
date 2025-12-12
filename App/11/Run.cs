namespace App._11
{
    internal class Run : IDay
    {
        private Dictionary<string, HashSet<string>> _deviceDict = [];

        public void Part1(IEnumerable<string> input)
        {
            ParseDevices(input);

            Dictionary<(string, (bool, bool)), long> cache = [];
            long paths = 0;

            FindPaths("you", cache, ref paths, true, true);

            Console.WriteLine(paths);
        }

        public void Part2(IEnumerable<string> input)
        {
            if (input.Count() < 100)
                input = """
                        svr: aaa bbb
                        aaa: fft
                        fft: ccc
                        bbb: tty
                        tty: ccc
                        ccc: ddd eee
                        ddd: hub
                        hub: fff
                        eee: dac
                        dac: fff
                        fff: ggg hhh
                        ggg: out
                        hhh: out
                    """.Split(Environment.NewLine).Select(l => l.Trim());

            ParseDevices(input);

            Dictionary<(string, (bool, bool)), long> cache = [];
            long paths = 0;

            FindPaths("svr", cache, ref paths);

            Console.WriteLine(paths);
        }

        private void ParseDevices(IEnumerable<string> input)
        {
            _deviceDict = input
                .Select(l =>
                {
                    string id = l[..l.IndexOf(':')];
                    string[] connections = l[(l.IndexOf(':') + 1)..].Split(' ');
                    return (id, connections);
                })
                .ToDictionary(
                    k => k.id,
                    v => v.connections.Where(a => !string.IsNullOrEmpty(a)).ToHashSet());

            _deviceDict["out"] = [];
        }

        private void FindPaths(
            string currentMachine,
            Dictionary<(string, (bool dac, bool fft)), long> cache,
            ref long counter,
            bool dacVisited = false,
            bool fftVisited = false)
        {
            (string, (bool, bool)) cacheKey = (currentMachine, (dacVisited, fftVisited));

            if (cache.TryGetValue(cacheKey, out long cached))
            {
                counter += cached;
                return;
            }

            if (currentMachine == "out")
            {
                counter += (dacVisited && fftVisited) ? 1 : 0;
                return;
            }

            if (currentMachine == "dac") dacVisited = true;
            if (currentMachine == "fft") fftVisited = true;

            long branchResult = 0;
            if (_deviceDict.TryGetValue(currentMachine, out HashSet<string>? outputs))
            {
                foreach (string output in outputs) FindPaths(output, cache, ref branchResult, dacVisited, fftVisited);
            }

            cache[cacheKey] = branchResult;
            counter += branchResult;
            return;
        }

        public void Reset() { }
    }
}
