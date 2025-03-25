using NetDream.Modules.OpenPlatform.Entities;
using NetDream.Modules.OpenPlatform.Models;
using NetDream.Shared.Http;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Securities;
using System;

namespace NetDream.Modules.OpenPlatform.Http
{
    public class PlatformResponse : IJsonResponse
    {
        private ISecurity? _encoder;
        private ISecurity? _decoder;
        private DateTime? _clientTime;
        private DateTime? _serverTime;


        public PlatformEntity? Platform { get; set; }

        public DateTime ClientTime {
            get => _clientTime ??= DateTime.Now;
            set {
                _decoder = null;
                _clientTime = value;
            }
        }
        public DateTime ServerTime { 
            get => _serverTime ??= DateTime.Now; 
            set {
                if (_encoder is not null)
                {
                    throw new ArgumentException(nameof(ServerTime));
                }
                _serverTime = value;
            }
        }

        public ISecurity Encoder => _encoder ??= new OwnEncoder(ServerTime);



        public ISecurity Decoder => _decoder ??= new OwnEncoder(ClientTime);


        public object Render(object data)
        {
            if (data is BaseResponse)
            {
                return data;
            }
            return new MetaResponse(data) 
            {
                Appid = Platform?.Appid,
                Timestamp = ServerTime.ToString()
            };
        }

        public object RenderData<T>(T data)
        {
            return Render(new DataOneResponse<T>(data)
            {
                Appid = Platform?.Appid,
                Timestamp = ServerTime.ToString()
            });
        }

        public object RenderData<T>(T data, string message)
        {
            return Render(new DataOneResponse<T>(data)
            {
                Appid = Platform?.Appid,
                Timestamp = ServerTime.ToString(),
                Message = message
            });
        }

        public object RenderFailure(string message, int code)
        {
            return new FailureResponse(code, message);
        }

        public object RenderFailure(string message)
        {
            return RenderFailure(message, 404);
        }

        public object RenderFailure(object message)
        {
            return new {
                code = 404,
                status = "failure",
                message
            };
        }

        public object RenderPage<T>(IPage<T> page)
        {

            return Render(new PageResponse<T>(page)
            {
                Appid = Platform?.Appid,
                Timestamp = ServerTime.ToString()
            });
        }
    }
}
