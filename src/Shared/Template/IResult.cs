using System.IO;

namespace NetDream.Shared.Template
{
    public interface IResult
    {
        public void Render(TextWriter writer, TemplateContext context);
    }
}
