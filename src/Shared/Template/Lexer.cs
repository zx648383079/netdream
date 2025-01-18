using System.IO;
using System.Text;

namespace NetDream.Shared.Template
{
    public partial class Lexer
    {
        public Lexer(string content)
            : this(new StringReader(content))
        {
            
        }

        public Lexer(TextReader reader)
        {
            Reader = reader;
        }


        protected Token ReadToken()
        {
            while (true)
            {
                var codeInt = ReadChar();
                if (codeInt == -1)
                {
                    return Token.Eof;
                }
                if (IsCodeExit(codeInt))
                {
                    return new Token(TokenType.CodeExit, "}", _position, _position);
                }
                var code = (char)codeInt;
                if (IsWhiteSpace(code))
                {
                    continue;
                }
                if (code is '\'' or '"')
                {
                    return GetStringToken(code);
                }
                if (code is '/' or '#' && IndexOf(code) > 0)
                {
                    return GetRegexToken(code);
                }
                if (code is >= '0' and <= '9')
                {
                    return GetNumericToken(code);
                }
                if (IsBracketOpen(codeInt))
                {
                    return new Token(TokenType.LeftBracket, code.ToString(), _position, _position);
                }
                if (IsBracketClose(codeInt))
                {
                    return new Token(TokenType.RightBracket, code.ToString(), _position, _position);
                }
                if (code == '.')
                {
                    var next = ReadChar();
                    if (next == '/')
                    {
                        return new Token(TokenType.Logic, "./", _position, _position);
                    }
                    if (next == '.')
                    {
                        next = ReadChar();
                        if (next == '/')
                        {
                            return new Token(TokenType.Logic, "../", _position, _position);
                        }
                        MoveBackChar();
                        return new Token(TokenType.Logic, "..", _position, _position);
                    }
                    MoveBackChar();
                    return new Token(TokenType.Dot, code.ToString(), _position, _position);
                }
                if (code == ':')
                {
                    return new Token(TokenType.Colon, code.ToString(), _position, _position);
                }
                if (code == ',')
                {
                    return new Token(TokenType.Comma, code.ToString(), _position, _position);
                }
                if (IsAlphabet(code) || code == '_' || code == '$')
                {
                    return GetNameToken(code);
                }
                if (code == ';')
                {
                    return new Token(TokenType.SemiColon, code.ToString(), _position, _position);
                }
                if (code == '=')
                {
                    var next = ReadChar();
                    if (next == '>')
                    {
                        return new Token(TokenType.Assign, "=>", _position, _position);
                    }
                    if (next == '=')
                    {
                        next = ReadChar();
                        if (next == '=')
                        {
                            return new Token(TokenType.Logic, "===", _position, _position);
                        }
                        MoveBackChar();
                        return new Token(TokenType.Logic, "==", _position, _position);
                    }
                    MoveBackChar();
                    return new Token(TokenType.Assign, code.ToString(), _position, _position);
                }
                if (code == '/')
                {
                    var next = ReadChar();
                    if (next == '=')
                    {
                        return new Token(TokenType.Logic, "/=", _position, _position);
                    }
                    MoveBackChar();
                    return new Token(TokenType.Operator, code.ToString(), _position, _position);
                }
                if (code == '!')
                {
                    var next = ReadChar();
                    if (next == '=')
                    {
                        return new Token(TokenType.Logic, "!=", _position, _position);
                    }
                    MoveBackChar();
                    return new Token(TokenType.Operator, code.ToString(), _position, _position);
                }
                if (code == '<')
                {
                    var next = ReadChar();
                    if (next == '=')
                    {
                        return new Token(TokenType.Logic, "<=", _position, _position);
                    }
                    if (next == '>')
                    {
                        return new Token(TokenType.Logic, "<>", _position, _position);
                    }
                    if (next == '<')
                    {
                        return new Token(TokenType.Logic, "<<", _position, _position);
                    }
                    MoveBackChar();
                    return new Token(TokenType.Operator, code.ToString(), _position, _position);
                }
                if (code == '>')
                {
                    var next = ReadChar();
                    if (next == '=')
                    {
                        return new Token(TokenType.Logic, ">=", _position, _position);
                    }
                    if (next == '>')
                    {
                        return new Token(TokenType.Logic, ">>", _position, _position);
                    }
                    MoveBackChar();
                    return new Token(TokenType.Operator, code.ToString(), _position, _position);
                }
                if (code == '>')
                {
                    var next = ReadChar();
                    if (next == '=')
                    {
                        return new Token(TokenType.Logic, ">=", _position, _position);
                    }
                    MoveBackChar();
                    return new Token(TokenType.Operator, code.ToString(), _position, _position);
                }
                if (code == '@')
                {
                    var next = ReadChar();
                    MoveBackChar();
                    return new Token(TokenType.Operator, code.ToString(), _position, _position);
                }
                if (code == '+')
                {
                    var next = ReadChar();
                    if (next == '=')
                    {
                        return new Token(TokenType.Logic, "+=", _position, _position);
                    }
                    MoveBackChar();
                    return new Token(TokenType.Operator, code.ToString(), _position, _position);
                }
                if (code == '-')
                {
                    var next = ReadChar();
                    if (next == '=')
                    {
                        return new Token(TokenType.Logic, "-=", _position, _position);
                    }
                    MoveBackChar();
                    return new Token(TokenType.Operator, code.ToString(), _position, _position);
                }
                if (code == '*')
                {
                    var next = ReadChar();
                    if (next == '=')
                    {
                        return new Token(TokenType.Logic, "*=", _position, _position);
                    }
                    MoveBackChar();
                    return new Token(TokenType.Operator, code.ToString(), _position, _position);
                }
                if (code == '/')
                {
                    var next = ReadChar();
                    if (next == '=')
                    {
                        return new Token(TokenType.Logic, "/=", _position, _position);
                    }
                    MoveBackChar();
                    return new Token(TokenType.Operator, code.ToString(), _position, _position);
                }
                if (code == '%')
                {
                    var next = ReadChar();
                    if (next == '=')
                    {
                        return new Token(TokenType.Logic, "%=", _position, _position);
                    }
                    MoveBackChar();
                    return new Token(TokenType.Operator, code.ToString(), _position, _position);
                }
                if (code == '^')
                {
                    var next = ReadChar();
                    if (next == '=')
                    {
                        return new Token(TokenType.Logic, "^=", _position, _position);
                    }
                    if (next == '^')
                    {
                        return new Token(TokenType.Logic, "^^", _position, _position);
                    }
                    MoveBackChar();
                    return new Token(TokenType.Operator, code.ToString(), _position, _position);
                }
                if (code == '&')
                {
                    var next = ReadChar();
                    if (next == '&')
                    {
                        return new Token(TokenType.Logic, "&&", _position, _position);
                    }
                    if (next == '=')
                    {
                        return new Token(TokenType.Logic, "&=", _position, _position);
                    }
                    MoveBackChar();
                    return new Token(TokenType.Operator, code.ToString(), _position, _position);
                }
                if (code == '?')
                {
                    var next = ReadChar();
                    if (next == '=')
                    {
                        return new Token(TokenType.Logic, "?=", _position, _position);
                    }
                    if (next == '?')
                    {
                        return new Token(TokenType.Logic, "??", _position, _position);
                    }
                    MoveBackChar();
                    return new Token(TokenType.Operator, code.ToString(), _position, _position);
                }
                if (code == '|')
                {
                    var next = ReadChar();
                    if (next == '=')
                    {
                        return new Token(TokenType.Logic, "|=", _position, _position);
                    }
                    if (next == '|')
                    {
                        return new Token(TokenType.Logic, "||", _position, _position);
                    }
                    MoveBackChar();
                    return new Token(TokenType.Operator, code.ToString(), _position, _position);
                }
            }
        }

        private Token GetNameToken(char code)
        {
            var start = _position;
            var sb = new StringBuilder();
            sb.Append(code);
            while (true)
            {
                var codeInt = ReadChar();
                if (codeInt < 0)
                {
                    break;
                }
                if (IsNewLine(codeInt))
                {
                    NextTokenQueue.Equals(new Token(TokenType.NewLine, _position, _position));
                    break;
                }
                var c = (char)codeInt;
                if (IsWhiteSpace(c))
                {
                    MoveBackChar();
                    break;
                }
                if (!(IsNumeric(codeInt) || IsAlphabet(codeInt)) || codeInt > 127)
                {
                    MoveBackChar();
                    break;
                }
                sb.Append(c);
            }
            return new Token(code == '$' ? TokenType.IdentifierSpecial : TokenType.Identifier, sb.ToString(), start, _position);
        }

        private Token GetNumericToken(char code)
        {
            var isHex = 0; // 是否是十六进制, 0 未判断 1 是小数 2 是进制
            var sb = new StringBuilder();
            var start = _position;
            sb.Append(code);
            while (true)
            {
                var codeInt = ReadChar();
                if (codeInt < 0)
                {
                    break;
                }
                if (codeInt == '.')
                {
                    var next = ReadChar();
                    isHex = 1;
                    sb.Append((char)codeInt);
                    if (next < 0)
                    {
                        break;
                    }
                    codeInt = next;
                }
                if (isHex == 0)
                {
                    if (codeInt != 'X' && codeInt != 'x' && !IsHexNumeric(codeInt))
                    {
                        MoveBackChar();
                        break;
                    }
                    if (!IsNumeric(codeInt))
                    {
                        isHex = 2;
                    }
                }
                else if (isHex == 1)
                {
                    if (!IsNumeric(codeInt))
                    {
                        MoveBackChar();
                        break;
                    }
                }
                else if (isHex == 2 && !IsHexNumeric(codeInt))
                {
                    MoveBackChar();
                    break;
                }
                sb.Append((char)codeInt);
            }
            return new Token(TokenType.String, sb.ToString(), start, _position);
        }

        private Token GetStringToken(char end)
        {
            var start = _position;
            return new Token(TokenType.String, GetStringBlock(end), start, _position);
        }
        private Token GetRegexToken(char end)
        {
            var start = _position;
            return new Token(TokenType.Regex, GetStringBlock(end), start, _position);
        }
        private string GetStringBlock(char end)
        {
            var reverseCount = 0;
            var sb = new StringBuilder();
            while (true)
            {
                var codeInt = ReadChar();
                if (codeInt < 0)
                {
                    break;
                }
                if (codeInt == end && reverseCount % 2 == 0)
                {
                    break;
                }
                if (codeInt == '\\')
                {
                    reverseCount++;
                    if (reverseCount == 2)
                    {
                        sb.Append((char)codeInt);
                        reverseCount = 0;
                    }
                    continue;
                }
                reverseCount = 0;
                sb.Append((char)codeInt);
            }
            return sb.ToString();
        }
    }
}
