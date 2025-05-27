using NetDream.Modules.OnlineMedia.Entities;
using NetDream.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NetDream.Modules.OnlineMedia.Importers
{
    public class TxtImporter(MediaContext db) : IExporter, IImporter<LiveEntity>
    {
        public string FileName => "live.txt";

        public string MimeType => "text/plain";



        public bool IsMatch(Stream input, string fileName)
        {
            return fileName.EndsWith(".txt") || fileName.EndsWith(".csv");
        }

        public IEnumerable<LiveEntity> Read(Stream input)
        {
            input.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(input);
            while (true)
            {
                var line = reader.ReadLine();
                if (line == null)
                {
                    break;
                }
                var args = line.Split(',', 2);
                if (args.Length != 2)
                {
                    continue;
                }
                yield return new LiveEntity()
                {
                    Title = args[0],
                    Source = args[1],
                };
            }
        }

        public void Write(Stream output)
        {
            var items = db.Lives.ToArray();
            var writer = new StreamWriter(output);
            foreach (var item in items)
            {
                writer.WriteLine($"{item.Title},{item.Source}");
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
