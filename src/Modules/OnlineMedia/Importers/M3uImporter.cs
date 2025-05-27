using NetDream.Modules.OnlineMedia.Entities;
using NetDream.Shared.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NetDream.Modules.OnlineMedia.Importers
{
    public class M3uImporter(MediaContext db) : IExporter, IImporter<LiveEntity>
    {
        public string FileName => "live.m3u";

        public string MimeType => "text/plain";



        public bool IsMatch(Stream input, string fileName)
        {
            if (!fileName.EndsWith(".m3u"))
            {
                return false;
            }
            input.Seek(0, SeekOrigin.Begin);
            var buffer = new byte[7];
            input.ReadExactly(buffer);
            return Encoding.ASCII.GetString(buffer) == "#EXTM3U";
        }

        public IEnumerable<LiveEntity> Read(Stream input)
        {
            input.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(input);
            reader.ReadLine();
            while (true)
            {
                var line = reader.ReadLine();
                if (line == null)
                {
                    break;
                }
                if (string.IsNullOrWhiteSpace(line) || !line.StartsWith("#EXTINF:"))
                {
                    continue;
                }
                var next = ReadSourceLine(reader);
                if (next == null)
                {
                    break;
                }
                yield return new LiveEntity()
                {
                    Source = next,
                    Title = line.Split(',', 2)[1]
                };
            }
        }

        private string? ReadSourceLine(StreamReader reader)
        {
            while (true)
            {
                var line = reader.ReadLine();
                if (line == null)
                {
                    return line;
                }
                if (line.StartsWith('#'))
                {
                    continue;
                }
                return line;
            }
        }

        public void Write(Stream output)
        {
            var items = db.Lives.ToArray();
            var writer = new StreamWriter(output);
            writer.WriteLine("#EXTM3U");
            foreach (var item in items)
            {
                writer.WriteLine($"#EXTINF:-1,{item.Source}");
                writer.WriteLine(item.Title);
            }
        }
        public void Dispose()
        {
        }
    }
}
