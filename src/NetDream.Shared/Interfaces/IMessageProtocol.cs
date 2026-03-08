using System;
using System.Collections.Generic;

namespace NetDream.Shared.Interfaces
{
    public interface IMessageProtocol
    {
        public string Encode(string text);

        public string Decode(string text);

        public IOperationResult<int> SendCode(string target, string templateName, string code, IDictionary<string, string>? extra = null);

        public IOperationResult<int> Send(string target, string templateName, IDictionary<string, string> data);

        public bool VerifyCode(string target, string templateName, string code, bool once = true);

        public void SendCustom(string target, string title, string content, bool isHtml = true);

        public void SendCustom(string target, string title, Func<string> cb, bool isHtml = true);

        public string GenerateCode(int length, bool isNumeric = true);
    }
}
