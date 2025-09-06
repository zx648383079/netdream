using NetDream.Modules.Counter.Entities;
using System.Collections.Generic;
using System.IO;

namespace NetDream.Modules.Counter.Importers
{
    public interface ILogImporter
    {
        public IEnumerable<LogEntity> Read(string[] filedItems, Stream input);

        public string[] ParseField(string text);
    }
}
