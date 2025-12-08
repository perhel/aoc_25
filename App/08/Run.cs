
namespace App._08
{
    internal readonly record struct JunctionBox(
        int Id, double X, double Y, double Z)
        : IComparable<JunctionBox>
    {
        public int CompareTo(JunctionBox other)
            => (Id, X, Y, Z).CompareTo((other.Id, other.X, other.Y, other.Z));

        public void Connect(int other, Dictionary<int, HashSet<int>> circuits)
        {
            // Slå ihop kretsarna
            circuits[this.Id].UnionWith(circuits[other]);

            // kopiera över den sammanslagna kretsen till alla inkluderade boxars kretsar,
            // så att alla boxar vet att de ingår i samma krets.
            // Resulterar i en massa dubbletter. Optimering??
            foreach (int boxId in circuits[other])
            {
                circuits[boxId] = circuits[this.Id];
            }
        }
    }

    internal class Run : IDay
    {
        private JunctionBox[] _junctionBoxes = [];
        private IEnumerable<(JunctionBox a, JunctionBox b)> _pairsByDistance = [];
        Dictionary<int, HashSet<int>> _circuits = [];

        public void Part1(IEnumerable<string> input)
        {
            InitJunctionBoxes(input);

            _pairsByDistance = GetJunctionBoxPairsByDistance(_junctionBoxes);

            InitCircuits();

            int connectionsToMake = 1000; // 10 för TEST, annars 1000
            foreach ((JunctionBox a, JunctionBox b) in _pairsByDistance.Take(connectionsToMake))
            {
                if (PartOfSameCircuit(a.Id, b.Id)) continue; // Med i samma krets

                a.Connect(b.Id, _circuits);
            }

            // Här finns en drös kretsar med samma innehåll (alla boxar i en krets har en egen identisk krets). 
            int result = _circuits.Values.Distinct() // Filtrera ut unika kretsar
                .OrderByDescending(set => set.Count)
                .Take(3)
                .Aggregate(1, (acc, n) => acc * n.Count);

            Console.WriteLine(result);
        }

        public void Part2(IEnumerable<string> input)
        {
            InitCircuits();

            int availableJunctionBoxes = _junctionBoxes.Length;
            double extensionCable = 0;

            foreach ((JunctionBox a, JunctionBox b) in _pairsByDistance.TakeWhile(_ => availableJunctionBoxes > 1))
            {
                if (PartOfSameCircuit(a.Id, b.Id)) continue; // Med i samma krets

                a.Connect(b.Id, _circuits);
                extensionCable = a.X * b.X;
                availableJunctionBoxes--;
            }

            Console.WriteLine(extensionCable);
        }

        // Skapa boxar
        private void InitJunctionBoxes(IEnumerable<string> input)
        {
            _junctionBoxes = [.. input
                .Select((l, i) => (arr: l.Split(',').Select(int.Parse).ToArray(), i))
                .Select(x => new JunctionBox(x.i, x.arr[0], x.arr[1], x.arr[2]))];
        }

        // Skapa en uppsättning kretsar för varje box, med box som enda element i kretsen
        private void InitCircuits()
        {
            _circuits = _junctionBoxes.ToDictionary(jb => jb.Id, jb => new HashSet<int>([jb.Id]));
        }

        private static IEnumerable<(JunctionBox a, JunctionBox b)> GetJunctionBoxPairsByDistance(JunctionBox[] junctionBoxes)
            => junctionBoxes.SelectMany(a => junctionBoxes.Select(b => (a, b))) // Generera par (inkl dubbletter och 'par med sig själv')
                  .Where(pair => pair.a.CompareTo(pair.b) < 0) // Behåll endast en uppsättning par, kasta även 'par med sig själv'
                  .OrderBy(pair => Distance(pair.a, pair.b)); // Sortera efter avstånd (ASC)

        private bool PartOfSameCircuit(int a, int b)
            => _circuits[a] == _circuits[b]; // Value i dictionary är en identisk HashSet för alla boxar i en krets

        private static double Distance(JunctionBox a, JunctionBox b)
        {
            double dX = Math.Pow(b.X - a.X, 2);
            double dY = Math.Pow(b.Y - a.Y, 2);
            double dZ = Math.Pow(b.Z - a.Z, 2);

            return Math.Sqrt(dX + dY + dZ);
        }

        public void Reset()
        {
            _circuits.Clear();
        }
    }
}
