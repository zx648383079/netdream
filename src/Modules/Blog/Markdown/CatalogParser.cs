using Markdig.Helpers;
using Markdig.Parsers;
using System.Linq;
using System.Text.RegularExpressions;

namespace NetDream.Modules.Blog.Markdown
{
    /// <summary>
    /// catalog:
    /// </summary>
    public partial class CatalogParser : InlineParser
    {
        public CatalogParser()
        {
            OpeningCharacters = [BeginTag[0]];
        }

        private const string BeginTag = "catalog:";

        public override bool Match(InlineProcessor processor, ref StringSlice slice)
        {
            if (!slice.PeekCharExtra(-1).IsWhiteSpaceOrZero())
            {
                return false;
            }
            var match = CatalogRegex().Match(slice.Text.Substring(slice.Start, slice.Length));
            if (!match.Success)
            {
                return false;
            }
            processor.Inline = new CatalogBlock(
                match.Value[BeginTag.Length..].Split(',').Select(i => int.Parse(i.Trim())).Where(i => i > 0)
                .Distinct().ToArray()
                )
            {
                Span =
                {
                    Start = processor.GetSourcePosition(slice.Start, out int line, out int column),
                },
                Line = line,
                Column = column,
            };
            processor.Inline.Span.End = processor.Inline.Span.Start + match.Value.Length - 1;
            slice.Start += match.Value.Length;
            return true;
        }

        [GeneratedRegex(@"^catalog:([\d, ]+)")]
        private static partial Regex CatalogRegex();
    }
}
