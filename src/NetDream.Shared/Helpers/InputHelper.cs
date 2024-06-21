using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;
using NetDream.Shared.Interfaces.Forms;

namespace NetDream.Shared.Helpers
{
    public static class InputHelper
    {
        /// <summary>
        /// 从表单提交获取结果
        /// </summary>
        /// <param name="inputItems"></param>
        /// <param name=""></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IDictionary<string, object> Value(IFormInput[] inputItems, IDictionary<string, object> data)
        {
            var items = new Dictionary<string, object>();
            foreach (var item in inputItems)
            {
                var name = item.Name;
                if (string.IsNullOrWhiteSpace(name)) {
                    continue;
                }
                if(data.TryGetValue(name, out var v)) {
                    items[name] = item.Filter(v);
                }
            }
            return items;
        }

        /// <summary>
        /// 给表单项赋值
        /// </summary>
        /// <param name="inputItems"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IFormInput[] Patch(IFormInput[] inputItems, object? data)
        {
            if (data is null)
            {
                return inputItems;
            }
            var items = new List<IFormInput>();
            foreach (var item in inputItems)
            {
                var name = item.Name;
                if (string.IsNullOrWhiteSpace(name)) {
                    continue;
                }
                if (data is IDictionary<string, object> o)
                {
                    if (o.TryGetValue(name, out var v))
                    {
                        item.Value = v;
                    }
                }
                else
                {
                    var field = data.GetType().GetProperty(StrHelper.Studly(name));
                    if (field != null)
                    {
                        item.Value = field.GetValue(data);
                    }
                }
                items.Add(item);
            }
            return [..items];
        }
    }
}
