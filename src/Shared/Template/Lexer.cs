using System;
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
            var start = _position;
            var last = CurrentToken;
            while (!NextIsCodeExit())
            {
                var codeInt = ReadChar();
                if (codeInt == -1)
                {
                    return Token.Eof;
                }
                var code = (char)codeInt;
                if (IsWhiteSpace(code))
                {
                    start = _position;
                    continue;
                }
                if (code is '\'' or '"')
                {
                    return GetStringToken(code);
                }
                if (last?.Type != TokenType.CodeEnter && code is '/' or '#' && IndexOf(code) > 0)
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
                        return new Token(TokenType.Logic, "./", start, _position);
                    }
                    if (next == '.')
                    {
                        next = ReadChar();
                        if (next == '/')
                        {
                            return new Token(TokenType.Logic, "../", start, _position);
                        }
                        MoveBackChar();
                        return new Token(TokenType.Logic, "..", start, _position);
                    }
                    MoveBackChar();
                    return new Token(TokenType.Dot, code.ToString(), _position, _position);
                }
                if (code == ':')
                {
                    if (last?.Type == TokenType.Identifier && !NextIsCodeExit())
                    {
                        var next = ReadChar();
                        if (next is '$' or '\'' or '"'  || IsWhiteSpace(next))
                        {
                            MoveBackChar();
                        } else
                        {
                            GetFormatStringToken();
                        }
                    }
                    return new Token(TokenType.Colon, code.ToString(), _position, _position);
                }
                if (code == ',')
                {
                    return new Token(TokenType.Comma, code.ToString(), _position, _position);
                }
                if (IsAlphabet(code) || code is '_' or '$')
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
                        return new Token(TokenType.Assign, "=>", start, _position);
                    }
                    if (next == '=')
                    {
                        next = ReadChar();
                        if (next == '=')
                        {
                            return new Token(TokenType.Logic, "===", start, _position);
                        }
                        MoveBackChar();
                        return new Token(TokenType.Logic, "==", start, _position);
                    }
                    MoveBackChar();
                    return new Token(TokenType.Assign, code.ToString(), _position, _position);
                }
                if (code == '/')
                {
                    var next = ReadChar();
                    if (next is '*' or '/')
                    {
                        return GetCommentToken(next is '*', start);
                    }
                    if (last?.Type == TokenType.CodeEnter)
                    {
                        MoveBackChar();
                        return new Token(TokenType.BlockExit, GetUntilCodeExit(), start, _position);
                    }
                    if (next == '=')
                    {
                        return new Token(TokenType.Logic, "/=", start, _position);
                    }
                    MoveBackChar();
                    return new Token(TokenType.Operator, code.ToString(), _position, _position);
                }
                if (code == '!')
                {
                    var next = ReadChar();
                    if (next == '=')
                    {
                        return new Token(TokenType.Logic, "!=", start, _position);
                    }
                    MoveBackChar();
                    return new Token(TokenType.Operator, code.ToString(), _position, _position);
                }
                if (code == '<')
                {
                    var next = ReadChar();
                    if (next == '=')
                    {
                        return new Token(TokenType.Logic, "<=", start, _position);
                    }
                    if (next == '>')
                    {
                        return new Token(TokenType.Logic, "<>", start, _position);
                    }
                    if (next == '<')
                    {
                        return new Token(TokenType.Logic, "<<", start, _position);
                    }
                    MoveBackChar();
                    return new Token(TokenType.Operator, code.ToString(), _position, _position);
                }
                if (code == '>')
                {
                    if (last?.Type == TokenType.CodeEnter)
                    {
                        return new Token(TokenType.BlockEnter, code.ToString(), start, _position);
                    }
                    var next = ReadChar();
                    if (next == '=')
                    {
                        return new Token(TokenType.Logic, ">=", start, _position);
                    }
                    if (next == '>')
                    {
                        return new Token(TokenType.Logic, ">>", start, _position);
                    }
                    MoveBackChar();
                    return new Token(TokenType.Operator, code.ToString(), _position, _position);
                }
                if (code == '@')
                {
                    if (last?.Type == TokenType.CodeEnter)
                    {
                        start = _position;
                        return new Token(TokenType.AssetImport, GetUntilCodeExit(), start, _position);
                    }
                    return new Token(TokenType.Operator, code.ToString(), _position, _position);
                }
                if (code == '+')
                {
                    if (last?.Type == TokenType.CodeEnter)
                    {
                        if (NextIsCodeExit())
                        {
                            return new Token(TokenType.BlockEnter, "else", _position, _position);

                        }
                        return new Token(TokenType.BlockEnter, "elseif", _position, _position);
                    }
                    var next = ReadChar();
                    if (next == '=')
                    {
                        return new Token(TokenType.Logic, "+=", start, _position);
                    }
                    if (next == '+')
                    {
                        return new Token(TokenType.Logic, "++", start, _position);
                    }
                    MoveBackChar();
                    return new Token(TokenType.Operator, code.ToString(), _position, _position);
                }
                if (code == '-')
                {
                    if (last?.Type == TokenType.CodeEnter)
                    {
                        return new Token(TokenType.BlockExit, "if", _position, _position);
                    }
                    var next = ReadChar();
                    if (next == '=')
                    {
                        return new Token(TokenType.Logic, "-=", start, _position);
                    }
                    if (next == '-')
                    {
                        return new Token(TokenType.Logic, "--", start, _position);
                    }
                    MoveBackChar();
                    return new Token(TokenType.Operator, code.ToString(), _position, _position);
                }
                if (code == '*')
                {
                    var next = ReadChar();
                    if (next == '=')
                    {
                        return new Token(TokenType.Logic, "*=", start, _position);
                    }
                    MoveBackChar();
                    return new Token(TokenType.Operator, code.ToString(), _position, _position);
                }
                if (code == '%')
                {
                    var next = ReadChar();
                    if (next == '=')
                    {
                        return new Token(TokenType.Logic, "%=", start, _position);
                    }
                    MoveBackChar();
                    return new Token(TokenType.Operator, code.ToString(), _position, _position);
                }
                if (code == '^')
                {
                    var next = ReadChar();
                    if (next == '=')
                    {
                        return new Token(TokenType.Logic, "^=", start, _position);
                    }
                    if (next == '^')
                    {
                        return new Token(TokenType.Logic, "^^", start, _position);
                    }
                    MoveBackChar();
                    return new Token(TokenType.Operator, code.ToString(), _position, _position);
                }
                if (code == '&')
                {
                    var next = ReadChar();
                    if (next == '&')
                    {
                        return new Token(TokenType.Logic, "&&", start, _position);
                    }
                    if (next == '=')
                    {
                        return new Token(TokenType.Logic, "&=", start, _position);
                    }
                    MoveBackChar();
                    return new Token(TokenType.Operator, code.ToString(), _position, _position);
                }
                if (code == '?')
                {
                    var next = ReadChar();
                    if (next == '=')
                    {
                        return new Token(TokenType.Logic, "?=", start, _position);
                    }
                    if (next == '?')
                    {
                        return new Token(TokenType.Logic, "??", start, _position);
                    }
                    if (next == ':')
                    {
                        return new Token(TokenType.Logic, "?:", start, _position);
                    }
                    MoveBackChar();
                    return new Token(TokenType.Operator, code.ToString(), _position, _position);
                }
                if (code == '|')
                {
                    if (last?.Type == TokenType.CodeEnter)
                    {
                        return new Token(TokenType.BlockEnter, "if", _position, _position);
                    }
                    var next = ReadChar();
                    if (next == '=')
                    {
                        return new Token(TokenType.Logic, "|=", start, _position);
                    }
                    if (next == '|')
                    {
                        return new Token(TokenType.Logic, "||", start, _position);
                    }
                    MoveBackChar();
                    return new Token(TokenType.Operator, code.ToString(), _position, _position);
                }
                if (code == '~')
                {
                    if (last?.Type == TokenType.CodeEnter)
                    {
                        return new Token(TokenType.BlockEnter, "for", _position, _position);
                    }
                    return new Token(TokenType.Operator, code.ToString(), _position, _position);
                }
                return new Token(TokenType.Unknown, code.ToString(), _position, _position);
            }
            return new Token(TokenType.Unknown, start, _position);
        }

        private Token GetCommentToken(bool isBlock, CursorPosition start)
        {
            if (!isBlock)
            {
                return new Token(TokenType.Comment, GetUntilCodeExit(), start, _position);
            }
            return new Token(TokenType.Comment, GetUntilCodeExit("*/"), start, _position);
        }

        private void GetFormatStringToken()
        {
            var sb = new StringBuilder();
            var start = _position;
            // sb.Append(code);
            while (true)
            {
                var codeInt = ReadChar();
                if (codeInt < 0)
                {
                    break;
                }
                if (IsCodeExit(codeInt) || codeInt == ',' || Is("=>"))
                {
                    MoveBackChar();
                    break;
                }
                if (codeInt == ':')
                {
                    if (sb.Length > 0 && ReadLineNextChar() == '$')
                    {
                        NextTokenQueue.Enqueue(new Token(TokenType.String, sb.ToString(), start, _position.NextColumn(-1)));
                        NextTokenQueue.Enqueue(new Token(TokenType.Operator, "+", _position, _position));
                        NextTokenQueue.Enqueue(GetNameToken((char)ReadChar()));
                        sb.Clear();
                        start = _position;
                        continue;
                    }
                    NextTokenQueue.Enqueue(new Token(TokenType.Operator, "+", _position, _position));
                    start = _position;
                    continue;
                }
                sb.Append((char)codeInt);
            }
            if (sb.Length > 0)
            {
                NextTokenQueue.Enqueue(new Token(TokenType.String, sb.ToString(), start, _position));
            }
        }

        private Token GetNameToken(char code)
        {
            var start = _position;
            var sb = new StringBuilder();
            sb.Append(code);
            while (!NextIsCodeExit())
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
                if (!IsWord(codeInt))
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

        private string GetUntilCodeExit(string end)
        {
            var sb = new StringBuilder();
            while (!NextIsCodeExit())
            {
                var codeInt = ReadChar();
                if (codeInt < 0)
                {
                    break;
                }
                if (Is(end))
                {
                    MoveInlineOffset(end.Length - 1);
                    break;
                }
                sb.Append((char)codeInt);
            }
            return sb.ToString();
        }

        private string GetUntilCodeExit()
        {
            var sb = new StringBuilder();
            while (!NextIsCodeExit())
            {
                var codeInt = ReadChar();
                if (codeInt < 0)
                {
                    break;
                }
                sb.Append((char)codeInt);
            }
            return sb.ToString();
        }
        private string GetStringBlock(char end)
        {
            var reverseCount = 0;
            var sb = new StringBuilder();
            while (!NextIsCodeExit())
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
