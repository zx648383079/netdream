using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Shared.Template
{
    public enum TokenType
    {
        /// <summary>
        /// None
        /// </summary>
        None,
        /// <summary>
        /// The text.
        /// </summary>
        Text,
        /// <summary>
        /// tag
        /// </summary>
        Tag,
        /// <summary>
        /// Start tag ${
        /// </summary>
        TagStart,
        /// <summary>
        /// End tag }
        /// </summary>
        TagEnd,
        /// <summary>
        /// string.
        /// </summary>
        String,
        /// <summary>
        /// number
        /// </summary>
        Number,
        /// <summary>
        /// Left Bracket [
        /// </summary>
        LeftBracket,
        /// <summary>
        /// Right Bracket ]
        /// </summary>
        RightBracket,
        /// <summary>
        /// Left Parentheses (
        /// </summary>
        LeftParentheses,
        /// <summary>
        /// Right Parentheses )
        /// </summary>
        RightParentheses,
        /// <summary>
        /// Left Brace {
        /// </summary>
        LeftBrace,
        /// <summary>
        /// Right Brace }
        /// </summary>
        RightBrace,
        /// <summary>
        /// New Line
        /// </summary>
        NewLine,
        /// <summary>
        /// Dot .
        /// </summary>
        Dot,
        /// <summary>
        /// Start String "
        /// </summary>
        StringStart,
        /// <summary>
        /// End String "
        /// </summary>
        StringEnd,
        /// <summary>
        /// Space
        /// </summary>
        Space,
        /// <summary>
        /// Punctuation (,:...)
        /// </summary>
        Punctuation,
        /// <summary>
        /// Operator (+,-,*,/,%..)
        /// </summary>
        Operator,
        /// <summary>
        /// <![CDATA[Logic Operator (>,>=,==,!=,<,<=,||,&&)]]> 
        /// </summary>
        Logic,
        /// <summary>
        /// Arithmetic Operator (+,-,*,/,%..)
        /// </summary>
        Arithmetic,
        /// <summary>
        /// Comma (,)
        /// </summary>
        Comma,
        /// <summary>
        /// Colon (:)
        /// </summary>
        Colon,
        /// <summary>
        /// Comment ($* comment *$)
        /// </summary>
        Comment,
        /// <summary>
        /// eof
        /// </summary>
        EOF
    }
}
