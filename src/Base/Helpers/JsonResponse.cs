using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetDream.Base.Helpers
{
    /// <summary>
    /// Ajax请求结果
    /// </summary>
    public class JsonResponse
    {
        /// <summary>
        /// 是否成功 200 表示成功
        /// </summary>
        public int Code { get; set; } = 200;

        public string Status { get; set; } = "sucess";

        /// <summary>
        /// 返回消息
        /// </summary>
        public object Messages { get; set; }

        public object Errors { get; set; }

        /// <summary>
        /// 返回数据
        /// </summary>
        public object Data { get; set; }

        public JsonResponse() { }
        /// <summary>
        /// 成功的简单返回
        /// </summary>
        /// <param name="data"></param>
        public JsonResponse(object data)
        {
            Code = 200;
            Status = "success";
            this.Data = data;
        }
        /// <summary>
        /// 成功的完整返回
        /// </summary>
        /// <param name="data"></param>
        /// <param name="messages"></param>
        public JsonResponse(object data, object messages)
        {
            Code = 200;
            Status = "success";
            this.Data = data;
            this.Messages = messages;
        }
        /// <summary>
        /// 失败的简单返回
        /// </summary>
        /// <param name="code"></param>
        /// <param name="errors"></param>
        public JsonResponse(int code, object messages)
        {
            this.Code = code;
            if (code == 200)
            {
                Status = "success";
                this.Messages = messages;
            }
            else
            {
                Status = "failure";
                this.Errors = messages;
            }

        }

        /// <summary>
        /// 失败的完整返回
        /// </summary>
        /// <param name="code"></param>
        /// <param name="status"></param>
        /// <param name="errors"></param>
        public JsonResponse(int code, string status, object messages)
        {
            this.Code = code;
            this.Status = status;
            if (code == 200)
            {

                this.Messages = messages;
            }
            else
            {
                this.Errors = messages;
            }
        }


        #region 业务返回

        /// <summary>
        /// 成功请求
        /// </summary>
        /// <returns></returns>
        public static JsonResponse Success()
        {
            return new JsonResponse();
        }
        /// <summary>
        /// 
        /// 成功请求
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static JsonResponse Success(object data)
        {
            return new JsonResponse(data);
        }
        /// <summary>
        /// 成功请求
        /// </summary>
        /// <param name="data"></param>
        /// <param name="messages"></param>
        /// <returns></returns>
        public static JsonResponse Success(object data, object messages)
        {
            return new JsonResponse(data, messages);
        }
        /// <summary>
        /// 失败请求
        /// </summary>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static JsonResponse Failure(object errors)
        {
            return new JsonResponse(400, errors);
        }

        /// <summary>
        /// 失败请求
        /// </summary>
        /// <param name="code">不允许设为200</param>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static JsonResponse Failure(int code, object errors)
        {
            if (code == 200)
            {
                code = 400;
            }
            return new JsonResponse(code, errors);
        }

        #endregion
    }
}
