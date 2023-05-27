using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.OpenPlatform.Models
{
    public class DataResponse<T>: DataOneResponse<IEnumerable<T>>
    {
        public DataResponse(IEnumerable<T> data): base(data)
        {
        }
    }

    public class DataOneResponse<T>
    {
        public string? Appid { get; set; } = string.Empty;

        public T Data { get; set; }

        public string Message { get; set; } = string.Empty;

        public DataOneResponse(T data)
        {
            Data = data;
        }
    }
}
