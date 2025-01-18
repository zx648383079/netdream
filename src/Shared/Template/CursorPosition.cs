using System;

namespace NetDream.Shared.Template
{
    public class CursorPosition : IEquatable<CursorPosition>
    {
        public static readonly CursorPosition Eof = new(-1, -1, -1);

        public CursorPosition()
            : this(0, 0, 0)
        {
            
        }

        public CursorPosition(int offset, int line, int column)
        {
            Offset = offset;
            Column = column;
            Line = line;
        }

        public int Offset { get; private set; }

        public int Column { get; private set; }

        public int Line { get; private set; }

        public CursorPosition NextColumn(int offset = 1)
        {
            return new CursorPosition(Offset + offset, Line, Column + offset);
        }

        public CursorPosition NextLine(int offset = 1)
        {
            return new CursorPosition(Offset + offset, Line + offset, 0);
        }

        public override string ToString()
        {
            return $"({Offset}:{Line},{Column})";
        }

        public string ToStringSimple()
        {
            return $"{Line + 1},{Column + 1}";
        }

        public bool Equals(CursorPosition other)
        {
            return Offset == other.Offset && Column == other.Column && Line == other.Line;
        }


        public override bool Equals(object? obj)
        {
            if (obj is null)
            {
                return false;
            }
            return obj is CursorPosition pos && Equals(pos);
        }

        public override int GetHashCode()
        {
            return Offset;
        }

        public static bool operator ==(CursorPosition left, CursorPosition right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CursorPosition left, CursorPosition right)
        {
            return !left.Equals(right);
        }
    }
}
