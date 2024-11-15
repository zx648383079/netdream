using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Shared.ZoDreamTemplate
{
    public class Lexer(TextReader reader)
    {


        public Token NextToken()
        {
            return ReadToken();
        }

        public Token ReadToken()
        {
            return null;
        }
    }
}
