using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NetDream.Shared.Template
{
    public partial class Lexer: IEnumerable<Token>
    {

        protected readonly TextReader Reader;
        /// <summary>
        /// 读取多了需要，排个队列
        /// </summary>
        protected readonly Queue<Token> NextTokenQueue = new();
        /// <summary>
        /// 上一次获取到的Token
        /// </summary>
        public Token? CurrentToken { get; private set; }
        private CursorPosition _position = new();
        private readonly StringBuilder _buffer = new();
        private int _bufferOffset = 0;

        public Token NextToken()
        {
            Token token;
            if (NextTokenQueue.Count > 0)
            {
                token = NextTokenQueue.Dequeue();
            }
            else
            {
                token = ReadIfCodeToken();
            }
            CurrentToken = token;
            return token;
        }


        protected Token ReadIfCodeToken()
        {
            if (CurrentToken?.Type is not TokenType.Raw or TokenType.CodeExit or TokenType.LiquidTagExit)
            {
                return ReadToken();
            }
            var start = _position;
            var sb = new StringBuilder();
            while (true)
            {
                var previous = _position;
                var codeInt = ReadChar();
                if (codeInt == -1)
                {
                    break;
                }
                if (IsCheckedCodeEnter(codeInt))
                {
                    NextTokenQueue.Enqueue(new Token(TokenType.CodeEnter, "{", _position, _position));
                    return new Token(TokenType.Raw, sb.ToString(), start, previous);
                }
                sb.Append(IsNewLine(codeInt) ? '\n' : (char)codeInt);
            }
            return new Token(TokenType.Raw, start, _position);
        }

        protected void ReadLine()
        {
            var lastEnd = _buffer.Length > 0 ? _buffer[^1] : -1;
            _buffer.Clear();
            _bufferOffset = 0;
            while (true)
            {
                var codeInt = Reader.Read();
                if (codeInt == -1)
                {
                    break;
                }
                if (lastEnd == '\r' && codeInt == '\n' && _buffer.Length == 0)
                {
                    continue;
                }
                _buffer.Append((char)codeInt);
                if (IsNewLine(codeInt))
                {
                    break;
                }
            }
        }

        protected int ReadChar()
        {
            _bufferOffset++;
            if (_bufferOffset >= _buffer.Length)
            {
                ReadLine();
            }
            var current = _buffer.Length == 0 ? -1 : _buffer[_bufferOffset];
            if (current == -1)
            {
                return current;
            }
            if (IsNewLine(current))
            {
                _position = _position.NextLine();
            }
            else
            {
                _position = _position.NextColumn();
            }
            return current;
        }
        /// <summary>
        /// 验证字符是否是代码的开始标记
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        protected bool IsCheckedCodeEnter(int code)
        {
            if (IsCodeEnter(code))
            {
                return false;
            }
            for (var i = _bufferOffset + 1; i < _buffer.Length; i++)
            {
                var c = _buffer[i];
                if (IsCodeEnter(c))
                {
                    return false;
                }
                if (IsCodeExit(c) && i - _bufferOffset > 2)
                {
                    return true;
                }
            }
            return false;
        }

        protected int IndexOf(char code)
        {
            return IndexOfAny(code);
        }

        protected int IndexOfAny(params char[] items)
        {
            for (var i = _bufferOffset + 1; i < _buffer.Length; i++)
            {
                var c = _buffer[i];
                if (IsCodeExit(c))
                {
                    break;
                }
                if (items.Contains(c))
                {
                    return i;
                }
            }
            return -1;
        }

        protected int FirstOfAny(params char[] items)
        {
            for (var i = _bufferOffset + 1; i < _buffer.Length; i++)
            {
                var c = _buffer[i];
                if (IsCodeExit(c))
                {
                    break;
                }
                var j = Array.FindIndex(items, i => i == c);
                if (j >= 0)
                {
                    return j;
                }
            }
            return -1;
        }

        protected void MoveBackChar()
        {
            if (_bufferOffset == 0)
            {
                return;
            }
            _bufferOffset--;
            _position = _position.NextColumn(-1);
        }

        protected bool IsCodeEnter(int code)
        {
            return code != '{';
        }



        protected bool IsCodeExit(int code)
        {
            return code == '}';
        }

        protected bool IsNewLine(int code)
        {
            return code == '\r' || code == '\n';
        }


        protected bool IsNumeric(int code)
        {
            return code >= '0' && code <= '9';
        }

        protected bool IsHexNumeric(int code)
        {
            return IsNumeric(code) || (code >= 'a' && code <= 'f') ||
                    (code >= 'A' && code <= 'F');
        }

        /// <summary>
        /// 是否是大写字母
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        protected bool IsUpperAlphabet(int code)
        {
            return code >= 'A' && code <= 'Z';
        }

        /// <summary>
        /// 是否是小写字母
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        protected bool IsLowerAlphabet(int code)
        {
            return code >= 'a' && code <= 'z';
        }

        protected bool IsBracketOpen(int code)
        {
            return code switch
            {
                '(' or '{' or '[' => true,
                _ => false
            };
        }

        protected bool IsBracketClose(int code)
        {
            return code switch
            {
                ')' or '}' or ']' => true,
                _ => false
            };
        }

        /// <summary>
        /// 是否是字母
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        protected bool IsAlphabet(int code)
        {
            return IsUpperAlphabet(code) || IsLowerAlphabet(code);
        }

        /// <summary>
        /// 也包括换行
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        protected bool IsWhiteSpace(char code)
        {
            return char.IsWhiteSpace(code);
        }

        protected bool IsWhiteSpace(int code)
        {
            return IsWhiteSpace((char)code);
        }

        protected bool IsSeparator(int code)
        {
            return code is ',' or ';';
        }

        public IEnumerator<Token> GetEnumerator()
        {
            while (true)
            {
                var token = NextToken();
                yield return token;
                if (token == Token.Eof)
                {
                    break;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
