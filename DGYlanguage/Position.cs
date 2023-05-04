public struct Position
{
    public uint Column;
    public uint Row;

    public Position(uint row, uint column)
    {
        Column = column;
        Row = row;
    }

    public static bool operator ==(Position a, Position b) => (a.Column == b.Column && a.Row == b.Row);
    public static bool operator !=(Position a, Position b) => !(a == b);
    public static bool operator >(Position a, Position b) => (a.Row > b.Row || (a.Row == b.Row && a.Column > b.Column));
    public static bool operator <(Position a, Position b) => (a.Row < b.Row || (a.Row == b.Row && a.Column < b.Column));
    public static bool operator >=(Position a, Position b) => (a == b || a > b);
    public static bool operator <=(Position a, Position b) => (a == b || a < b);

    public override bool Equals(object obj) => (obj is Position other && this == other);
    public override int GetHashCode() => HashCode.Combine(Column, Row);
}
