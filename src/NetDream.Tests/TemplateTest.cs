using NetDream.Shared.Template;
using System;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;

namespace NetDream.Tests
{
    [TestClass]
    public class TemplateTest
    {
        [TestMethod]
        //[DataRow("{if:isset($keywords),$keywords}", 2)]
        [DataRow("{contentPage:channel=>product_sample,product_id=>$data.id,field=>'id,cat_id,model_id,title,file',num=>5}\r\n{$content.title}\r\n{/contentPage}", 7)]
        public void TestLexer(string template, int tokenCount)
        {
            var reader = new Lexer(template);

            var items = new List<Token>();
            var token = reader.NextToken();
            items.Add(token);
            // Assert.AreEqual(token.Value, "get");
            while (true)
            {
                token = reader.NextToken();
                items.Add(token);
                if (token == Token.Eof)
                {
                    break;
                }
            }
            Assert.IsTrue(items.Count == tokenCount);
        }
    }
}