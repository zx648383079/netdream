using NetDream.Modules.OnlineMedia.Entities;
using NetDream.Shared.Interfaces;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace NetDream.Modules.OnlineMedia.Importers
{
    public class DplImporter(MediaContext db) : IExporter, IImporter<LiveEntity>
    {
        public string FileName => "live.dpl";

        public string MimeType => "text/plain";



        public bool IsMatch(Stream input, string fileName)
        {
            if (!fileName.EndsWith(".dpl"))
            {
                return false;
            }
            input.Seek(0, SeekOrigin.Begin);
            var buffer = new byte[12];
            input.ReadExactly(buffer);
            return Encoding.ASCII.GetString(buffer) == "DAUMPLAYLIST";
        }

        public IEnumerable<LiveEntity> Read(Stream input)
        {
            input.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(input);
            reader.ReadLine();
            while (true)
            {
                var line = ReadLine(reader, "*file*");
                if (line == null)
                {
                    break;
                }
                var i = line.IndexOf("*file*");
                Debug.Assert(i > 0);
                var next = ReadLine(reader, "*title*");
                if (string.IsNullOrWhiteSpace(next))
                {
                    break;
                }
                var j = next.IndexOf("*title*");
                Debug.Assert(j > 0);
                yield return new LiveEntity()
                {
                    Source = line[(i + 6)..],
                    Title = next[(j + 7)..]
                };
            }
        }

        private string? ReadLine(StreamReader reader, string tag)
        {
            while (true)
            {
                var line = reader.ReadLine();
                if (line == null)
                {
                    return line;
                }
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                if (line.IndexOf(tag) > 0)
                {
                    return line;
                }
            }
        }

        public void Write(Stream output)
        {
            var items = db.Lives.ToArray();
            var writer = new StreamWriter(output);
            writer.WriteLine("DAUMPLAYLIST");
            var i = 0;
            foreach (var item in items)
            {
                i++;
                writer.WriteLine($"{i}*file*{item.Source}");
                writer.WriteLine($"{i}*title*{item.Title}");
            }
        }

        public void Dispose()
        {
        }
    }
}
