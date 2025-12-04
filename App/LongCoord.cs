namespace App
{
    public readonly struct LongCoord : IEquatable<LongCoord>
    {
        private readonly long _value;

        private LongCoord(long value)
        {
            _value = value;
        }

        public static LongCoord FromXY(int x, int y)
        {
            long encoded = ((long)x << 32) | (uint)y;
            return new(encoded);
        }

        public int X => (int)(_value >> 32);
        public int Y => (int)_value;

        public static implicit operator long(LongCoord coord) => coord._value;
        public static explicit operator LongCoord(long value) => new(value);

        public static bool operator ==(LongCoord left, LongCoord right) => left.Equals(right);
        public static bool operator !=(LongCoord left, LongCoord right) => !left.Equals(right);

        public bool Equals(LongCoord other) => _value == other._value;
        public override bool Equals(object? obj) => obj is LongCoord other && Equals(other);
        public override int GetHashCode() => _value.GetHashCode();
        public override string ToString() => $"{X}, {Y}";
    }
}