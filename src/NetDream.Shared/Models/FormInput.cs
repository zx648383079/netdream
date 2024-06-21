using Microsoft.VisualBasic.FileIO;
using NetDream.Shared.Interfaces.Forms;
using NPoco.fastJSON;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NetDream.Shared.Models
{
    public class FormInput: IFormInput
    {
        public readonly string[] TYPE_ITEMS = ["text",
            "date",
            "datetime",
            "numeric",
            "email",
            "url",
            "password",
            "file",
            "tel",
            "image",
            "radio",
            "checkbox",
            "select", "textarea", "switch", "html",
            "markdown"
        ];
        public readonly string[] PROPERTY_ITEMS = ["id", "class", "type", "label", "name", "tip", "items", "value",
        "required", "placeholder", "multiple", "tooltip"];

        protected Dictionary<string, object> _data = [];
        protected Dictionary<string, object> _option = [];

        public string Name => _data["name"]?.ToString() ?? string.Empty;

        public object? Value 
        { 
            get {
                return Get("value");
            }
            set {
                Attr("value", value!);
            }
        }

        public IFormInput SetType(string type, string? name = null, object value = null) {
            _data.TryAdd("type", value);
            if (!string.IsNullOrWhiteSpace(name))
            {
                _data.TryAdd("name", name);
            }
            if (value is not null)
            {
                _data.TryAdd("value", value);
            }
            return this;
        }

        public IFormInput Attr(string key, object value)
        {
            _option.TryAdd(key, value);
            return this;
        }

        /// <summary>
        /// 转化请求的值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public object Filter(object value)
        {
            return value;
        }

        public IDictionary<string, object> ToArray()
        {
            var data = new Dictionary<string, object>();
            foreach (var item in _data)
            {
                data.Add(item.Key, item.Value);
            }
            if (_option.Count > 0)
            {
                data.TryAdd("option", _option);
            }
            return data;
        }

        public string ToJson()
        {
            return JsonSerializer.Serialize(ToArray());
        }


        //public string ToString()
        //{
        //    app().singletonIf(IHtmlRenderer.class, DarkRenderer.class);
        //    return app(IHtmlRenderer.class).renderInput(this);
        //}

        public void Set(string name, object value)
        {
            if (PROPERTY_ITEMS.Contains(name))
            {
                _data.TryAdd(name, value);
                return;
            }
            if (name == "option")
            {
                if (value is IDictionary<string, object> o)
                {
                    foreach (var item in o)
                    {
                        _option.TryAdd(item.Key, item.Value);
                    }
                }
                return;
            }
            _option.TryAdd(name, value);
        }

        public object? Get(string name)
        {
            if (PROPERTY_ITEMS.Contains(name))
            {
                return _data[name];
            }
            if (name == "option")
            {
                return _option;
            }
            return _option[name];
        }

        public FormInput()
        {
            
        }

        public FormInput(Dictionary<string, object> data)
        {
            _data = data;
        }

        public static IFormInput Text(string name, string label, bool required = false)
        {
            return new FormInput(new Dictionary<string, object>
            {
                {"type", "text" },
                {"name", name },
                {"label", label },
                {"required", required },
            });
        }

        public static IFormInput Date(string name, string label, bool required = false)
        {
            return new FormInput(new Dictionary<string, object>
            {
                {"type", "date" },
                {"name", name },
                {"label", label },
                {"required", required },
            });
        }

        public static IFormInput Datetime(string name, string label, bool required = false)
        {
            return new FormInput(new Dictionary<string, object>
            {
                {"type", "datetime" },
                {"name", name },
                {"label", label },
                {"required", required },
            });
        }

        public static IFormInput Password(string name, string label, bool required = false)
        {
            return new FormInput(new Dictionary<string, object>
            {
                {"type", "password" },
                {"name", name },
                {"label", label },
                {"required", required },
            });
        }

        public static IFormInput Url(string name, string label, bool required = false)
        {
            return new FormInput(new Dictionary<string, object>
            {
                {"type", "url" },
                {"name", name },
                {"label", label },
                {"required", required },
            });
        }

        public static IFormInput Tel(string name, string label, bool required = false)
        {
            return new FormInput(new Dictionary<string, object>
            {
                {"type", "tel" },
                {"name", name },
                {"label", label },
                {"required", required },
            });
        }

        public static IFormInput Number(string name, string label, bool required = false)
        {
            return new FormInput(new Dictionary<string, object>
            {
                {"type", "number" },
                {"name", name },
                {"label", label },
                {"required", required },
            });
        }

        public static IFormInput Color(string name, string label, bool required = false)
        {
            return new FormInput(new Dictionary<string, object>
            {
                {"type", "color" },
                {"name", name },
                {"label", label },
                {"required", required },
            });
        }

        public static IFormInput Textarea(string name, string label, bool required = false)
        {
            return new FormInput(new Dictionary<string, object>
            {
                {"type", "textarea" },
                {"name", name },
                {"label", label },
                {"required", required },
            });
        }

        public static IFormInput Email(string name, string label, bool required = false)
        {
            return new FormInput(new Dictionary<string, object>
            {
                {"type", "email" },
                {"name", name },
                {"label", label },
                {"required", required },
            });
        }

        public static IFormInput Switch(string name, string label)
        {
            return new FormInput(new Dictionary<string, object>
            {
                {"type", "switch" },
                {"name", name },
                {"label", label },
            });
        }

        public static IFormInput File(string name, string label, bool required = false)
        {
            return new FormInput(new Dictionary<string, object>
            {
                {"type", "file" },
                {"name", name },
                {"label", label },
                {"required", required },
            });
        }

        public static IFormInput Image(string name, string label, bool required = false)
        {
            return new FormInput(new Dictionary<string, object>
            {
                {"type", "image" },
                {"name", name },
                {"label", label },
                {"required", required },
            });
        }

        public static IFormInput Html(string name, string label, bool required = false)
        {
            return new FormInput(new Dictionary<string, object>
            {
                {"type", "html" },
                {"name", name },
                {"label", label },
                {"required", required },
            });
        }

        public static IFormInput Markdown(string name, string label, bool required = false)
        {
            return new FormInput(new Dictionary<string, object>
            {
                {"type", "markdown" },
                {"name", name },
                {"label", label },
                {"required", required },
            });
        }

        public static IFormInput Radio(string name, string label, string[] items, bool required = false)
        {
            return ItemsControl("radio", name, label, items, required);
        }

        public static IFormInput Radio(string name, string label, IList<OptionItem> items, bool required = false)
        {
            return ItemsControl("radio", name, label, items, required);
        }

        public static IFormInput ItemsControl(string type, string name, string label, string[] items, bool required = false)
        {
            return ItemsControl(type, name, label, items.Select((l, key) => new OptionItem(l, key)).ToArray(), required);
        }

        public static IFormInput ItemsControl(string type, string name, string label, IDictionary<object, string> items, bool required = false)
        {
            return ItemsControl(type, name, label, items.Select(item => new OptionItem(item.Value, item.Key)).ToArray(), required);
        }

        public static IFormInput ItemsControl(string type, string name, 
            string label, IList<OptionItem> items, bool required = false)
        {
            return new FormInput(new Dictionary<string, object>
            {
                {"type", type },
                {"name", name },
                {"label", label },
                {"items", items },
                {"required", required },
            });
        }

        public static IFormInput Checkbox(string name, string label, IList<OptionItem> items, bool required = false)
        {
            return ItemsControl("checkbox", name, label, items, required);
        }

        public static IFormInput Select(string name, string label, IList<OptionItem> items, bool required = false)
        {
            return ItemsControl("checkbox", name, label, items, required);
        }

        public static IFormInput Group(string label, IList<IFormInput> items)
        {
            return new FormInput(new Dictionary<string, object>
            {
                {"type", "group" },
                {"label", label },
                {"items", items },
            });
        }

    }
}
