using NetDream.Shared.Converters;
using System.Collections.Generic;

namespace NetDream.Modules.OpenPlatform
{
    public class BaseResponse
    {
        public string? Appid { get; set; } = string.Empty;

        public string Sign { get; set; } = string.Empty;

        public string? SignType { get; set; }

        public string Timestamp { get; set; } = string.Empty;

        public string? Encrypt { get; set; }
        public string? EncryptType { get; set; }

        public string? Message { get; set; }
    }
    public class MetaResponse : BaseResponse
    {

        [JsonMeta]
        public object? ExtraData { get; set; }

        public MetaResponse()
        {
            
        }

        public MetaResponse(object data)
        {
            ExtraData = data;
        }
    }
    public class DataOneResponse<T> : BaseResponse
    {
        public T Data { get; set; }

        public DataOneResponse(T data)
        {
            Data = data;
        }
    }

    public class DataResponse<T>: DataOneResponse<IEnumerable<T>>
    {
        public DataResponse(IEnumerable<T> data): base(data)
        {
        }
    }


}
