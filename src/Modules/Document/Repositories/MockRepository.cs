using NetDream.Modules.Document.Entities;
using NetDream.Modules.Document.Forms;
using NetDream.Modules.Document.Models;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace NetDream.Modules.Document.Repositories
{
    public class MockRepository(DocumentContext db, IClientContext client)
    {
        public IOperationResult<RequestResult> Request(RequestForm data)
        {

            return OperationResult.Fail<RequestResult>("");
        }

        public object? MockValue(string mock)
        {
            if (string.IsNullOrWhiteSpace(mock))
            {
                return null;
            }
            var data = mock.Split('|');
            var type = data[0];
            if (string.IsNullOrWhiteSpace(type))
            {
                return mock;
            }
            var rule = data[1] ?? string.Empty;
            var value = data[2] ?? string.Empty;
            return mock;
            //mock = new MockRule();
            //if (type == "array")
            //{
            //    type = "arr";
            //}
            //if (!method_exists(mock, type))
            //{
            //    return mock;
            //}
            //return mock.type(rule, value);
        }

        public Dictionary<string, object> GetDefaultData(int api_id, int parent_id = 0)
        {
            var fields = db.Fields.Where(i => i.Kind == 
                ApiRepository.KIND_RESPONSE && i.ApiId == api_id && i.ParentId == parent_id)
                .ToArray();
            var data = new Dictionary<string, object>();
            foreach (var v in fields)
            {
                var name = v.Name;
                if (v.Type == "array")
                {
                    data.Add(name, new object[] { GetDefaultData(api_id, v.Id) });
                    continue;
                }
                if (v.Type == "object")
                {
                    data[name] = GetDefaultData(api_id, v.Id);
                    continue;
                }
                if (string.IsNullOrWhiteSpace(v.DefaultValue) && !string.IsNullOrWhiteSpace(v.Mock))
                {
                    //v.SetMock();
                }
                data[name] = Format(v.Type, v.DefaultValue);
            }
            return data;

        }

        public object Format(string type, string val)
        {
            if (type == "number")
            {
                return float.Parse(val);
            }
            if (type == "boolean")
            {
                return val is "true" or "1";
            }
            return val;
        }

        /// <summary>
        /// 获取响应字段mock数组
        /// </summary>
        /// <param name="apiId"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public IOperationResult<Dictionary<string, object>> GetMockData(int apiId, int parentId = 0)
        {
            var fields = db.Fields.Where(i => i.Kind ==
                ApiRepository.KIND_RESPONSE && i.ApiId == apiId && i.ParentId == parentId)
                .ToArray();
            var data = new Dictionary<string, object>();
            return OperationResult.Ok(data);

        }

        public IOperationResult<FieldEntity[]> ParseContent(string content, 
            int kind = ApiRepository.KIND_REQUEST)
        {
            var res = new List<FieldEntity>();

            return OperationResult.Ok(res.ToArray());
        }

        public void ParseChildren(string? content, FieldEntity model)
        {
            if (string.IsNullOrEmpty(content))
            {
                model.Type = "null";
                return;
            }
            if (Validator.IsBoolean(content))
            {
                model.Type = "boolean";
                return;
            }
            if (Validator.IsFloat(content))
            {
                model.Type = "float";
                return;
            }
            if (Validator.IsDouble(content))
            {
                model.Type = "double";
                return;
            }
            if (Validator.IsNumeric(content))
            {
                model.Type = !content.Contains('.') ? "number" : "float";
                return;
            }
            if (content.StartsWith('[') && content.EndsWith(']'))
            {
                return;
            }
            model.DefaultValue = string.Empty;
        }

        public FieldEntity[] ParseHeader(string content)
        {
            var data = new List<FieldEntity>();
            foreach (var line in content.Split('\n'))
            {
                if (!line.Contains(':'))
                {
                    continue;
                }
                var args = line.Split(':', 2);
                var key = args[0].Trim();
                var value = args[1].Trim();
                if (string.IsNullOrEmpty(key))
                {
                    continue;
                }
                data.Add(new()
                {
                    Name = key,
                    Title = key,
                    DefaultValue = value,
                    Type = "string",
                    Kind = ApiRepository.KIND_HEADER,
                    IsRequired = !string.IsNullOrEmpty(value)
                });
            }
            return [..data];
        }
    }
}