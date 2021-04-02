using Microsoft.AspNetCore.Mvc;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetDream.Web.Base.Http
{
    public class JsonResponse : IJsonResponse
    {
        public object Render(object data)
        {
            return data;
        }

        public object RenderData(object data)
        {
            return Render(new
            {
                code = 200,
                status = "success",
                data
            });
        }

        public object RenderData(object data, string message)
        {
            return Render(new
            {
                code = 200,
                status = "success",
                data,
                message
            });
        }

        public object RenderFailure(string message, int code)
        {
            return new
            {
                code,
                status = "failure",
                message
            };
        }

        public object RenderFailure(string message)
        {
            return RenderFailure(message, 404);
        }

        public object RenderPage<T>(Page<T> page)
        {

            return Render(new
            {
                code = 200,
                status = "success",
                data = page.Items,
                paging = new
                {
                    limit = page.ItemsPerPage,
                    offset = page.CurrentPage,
                    total = page.TotalItems,
                    more = page.CurrentPage < page.TotalPages
                }
            });
        }
    }
}
