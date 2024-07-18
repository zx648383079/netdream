using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Shared.ZoDreamTemplate
{
    public class Token(TokenType type, string value)
    {
        public TokenType Type { get; private set; } = type;

        public string Value { get; private set; } = value;
    }
}
