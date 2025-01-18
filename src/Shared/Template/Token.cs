namespace NetDream.Shared.Template
{
    public class Token
    {
        public static readonly Token Eof = new(TokenType.Eof, CursorPosition.Eof, CursorPosition.Eof);
        public Token(TokenType type, string value, CursorPosition start, CursorPosition end)
            : this(type, start, end)
        {
            Value = value;
        }

        public Token(TokenType type, CursorPosition start, CursorPosition end)
        {
            Type = type;
            Begin = start;
            End = end;
        }

        public TokenType Type { get; private set; }

        public CursorPosition Begin { get; set; }

        public CursorPosition End { get; set; }

        public string Value { get; private set; } = string.Empty;

        public override string ToString()
            => string.Format("{0}='{1}'", Type, Value);

        public bool Equals(Token other)
        {
            return Type == other.Type && Value.Equals(other.Value);
        }

        public override bool Equals(object? obj)
        {
            if (obj is null)
            {
                return false;
            }
            return obj is Token token && Equals(token);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(Token left, Token right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Token left, Token right)
        {
            return !left.Equals(right);
        }
    }
}
