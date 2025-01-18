namespace NetDream.Shared.Template
{
    public enum TokenType
    {
        /// <summary>
        /// None
        /// </summary>
        None,

        /// <summary>Token "{{"</summary>
        CodeEnter,

        /// <summary>Token "{%"</summary>
        LiquidTagEnter,

        /// <summary>Token "}}"</summary>
        CodeExit,

        /// <summary>Token "%}"</summary>
        LiquidTagExit,

        Raw,

        /// <summary>
        /// string.
        /// </summary>
        String,
        /// <summary>
        /// number
        /// </summary>
        Number,

        /// <summary>
        /// An identifier starting by a $
        /// </summary>
        IdentifierSpecial,

        /// <summary>
        /// An identifier
        /// </summary>
        Identifier,

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
        /// Operator (+,-,*,/,%..)
        /// </summary>
        Operator,
        /// <summary>
        /// <![CDATA[Logic Operator (>,>=,==,!=,<,<=,||,&&)]]> 
        /// </summary>
        Logic,
        /// <summary>Token ";"</summary>
        SemiColon,

        Assign,

        /// <summary>
        /// Comma (,)
        /// </summary>
        Comma,
        /// <summary>
        /// Colon (:)
        /// </summary>
        Colon,

        Regex,
        /// <summary>
        /// Comment ($* comment *$)
        /// </summary>
        Comment,
        /// <summary>
        /// eof
        /// </summary>
        Eof,
        
    }
}
