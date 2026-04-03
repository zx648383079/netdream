using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace NetDream.Modules.Blog.Markdown
{
    public partial class SourceCodeTraceRenderer : HtmlObjectRenderer<CodeBlock>
    {
        protected override void Write(HtmlRenderer renderer, CodeBlock obj)
        {
            var language = string.Empty;
            var beginLineNo = 1;
            var highlightLines = new List<int>();
            var sourceUrl = string.Empty;
            if (obj is FencedCodeBlock f)
            {
                language = f.Info;
                if (!string.IsNullOrWhiteSpace(f.Arguments))
                {
                    var matches = TraceRegex().Matches(f.Arguments);
                    var i = 0;
                    foreach (Match item in matches)
                    {
                        i++;
                        if (item.Value[0] == '(')
                        {
                            sourceUrl = item.Value[1..^1];
                            continue;
                        }
                        if (item.Value.Contains(','))
                        {
                            highlightLines.AddRange(
                                item.Value[1..^1].Split(',').Select(int.Parse).Where(i => i > 0));
                            continue;
                        }
                        var args = item.Value[1..^1].Split("-").Select(int.Parse).ToArray();
                        if (args.Length > 2)
                        {
                            // 无效
                            continue;
                        }
                        if (args.Length == 1 || args[1] == 0)
                        {
                            if (i == 1 && matches.Count >= 2)
                            {
                                beginLineNo = args[0];
                                continue;
                            }
                        }
                        highlightLines.Add(args[0]);
                        if (args.Length == 1)
                        {
                            continue;
                        }
                        if (args[1] == 0)
                        {
                            args[1] = beginLineNo + obj.Lines.Count - 1;
                        }
                        if (args[0] > args[1])
                        {
                            // 不对
                            continue;
                        }
                        for (int j = args[0] + 1; j <= args[1]; j++)
                        {
                            highlightLines.Add(j);
                        }
                    }
                }
            }

            renderer.EnsureLine();
            // 开始代码块容器
            renderer.Write("<div class=\"code-container\">");
            renderer.Write("<div class=\"code-header\">");
            renderer.Write("<a data-action=\"copy\" title=\"copy\"><i class=\"icon-copy\"></i></a>");
            renderer.Write("<a data-action=\"full\" title=\"full screen\"><i class=\"icon-full-screen\"></i></a>");
            if (!string.IsNullOrWhiteSpace(sourceUrl))
            {
                renderer.Write($"<a href=\"{sourceUrl}\" target=\"_blank\" rel=\"noopener\" title=\"open url\"><i class=\"icon-cloud\"></i></a>");
            }

            if (!string.IsNullOrWhiteSpace(language))
            {
                renderer.Write($"<span>{language}</span>");
            }
            
            renderer.Write("</div>");
            // 添加行号容器
            renderer.Write("<div class=\"highlight-bar\">");
            for (int i = 0; i < obj.Lines.Count; i++)
            {
                if (highlightLines.Contains(beginLineNo + i))
                {
                    renderer.Write("<span class=\"highlighted\">&nbsp;</span>");
                } else
                {
                    renderer.Write("<span>&nbsp;</span>");
                }
            }
            renderer.Write("</div>");
            // 添加代码内容
            renderer.Write("<pre><code>");
            renderer.WriteLeafRawLines(obj, true, true);
            renderer.Write("</code></pre>");
            renderer.Write("<div class=\"line-number-bar\">");
            for (int i = 0; i < obj.Lines.Count; i++)
            {
                renderer.Write($"<span>{beginLineNo + i}</span>");
            }
            renderer.Write("</div>");
            renderer.Write("</div>");
            renderer.EnsureLine();
        }

        [GeneratedRegex(@"\{[\d-,]+\}|\(.+?\)")]
        private static partial Regex TraceRegex();
    }
}
