using System.IO;

namespace NetDream.Shared.Template
{
    public interface ITemplate
    {

        public void Render(TextWriter writer);
    }
}
