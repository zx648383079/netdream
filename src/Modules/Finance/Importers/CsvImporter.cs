using NetDream.Modules.Finance.Entities;
using NetDream.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NetDream.Modules.Finance.Importers
{
    public abstract class CsvImporter : IImporter<LogEntity>
    {
        
        private readonly byte[] _buffer = new byte[1024];
        public virtual Encoding Encoding => Encoding.UTF8;


        public abstract bool IsMatch(Stream input, string fileName);

        protected string? ReadLine(Stream input)
        {
            var count = 0;
            var isEnd = false;
            while (true)
            {
                var code = input.ReadByte();
                if (code < 0)
                {
                    isEnd = true;
                    break;
                }
                if (code is '\r' or '\n')
                {
                    if (count > 0)
                    {
                        break;
                    }
                    continue;
                }
                _buffer[count++] = (byte)code;
            }
            if (count == 0)
            {
                return isEnd ? null : string.Empty;
            }
            return Encoding.GetString(_buffer, 0, count).Trim();
        }

        protected bool FirstRowContains(Stream input, string tag)
        {
            input.Seek(0, SeekOrigin.Begin);
            var line = ReadLine(input);
            return line?.Contains(tag) == true;
        }

        public IEnumerable<LogEntity> Read(Stream input)
        {
            input.Seek(0, SeekOrigin.Begin);
            Ready();
            var status = 0;
            var column = Array.Empty<string>();
            while (true)
            {
                var line = ReadLine(input);
                if (line is null)
                {
                    break;
                }
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }
                if (status == 0)
                {
                    if (line.StartsWith("---"))
                    {
                        status = 1;
                    }
                    continue;
                }
                if (line.StartsWith("---"))
                {
                    break;
                }
                var data = line.Split(',').Select(i => i.Trim()).ToArray();
                if (status == 1)
                {
                    column = data;
                    status = 2;
                    continue;
                }
                var res = FormatData(column, data);
                if (res is null)
                {
                    continue;
                }
                yield return res;
            }
        }


        protected virtual void Ready()
        {

        }

        protected abstract LogEntity? FormatData(string[] columns, string[] data);

        protected string GetValue(string[] columns, string[] data, string key)
        {
            var i = Array.FindIndex(columns, x => x == key);
            if (i >= 0)
            {
                return data[i];
            }
            return string.Empty;
        }

        public void Dispose()
        {
        }
    }
}
