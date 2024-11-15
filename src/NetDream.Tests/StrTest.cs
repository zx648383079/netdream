using NetDream.Shared.Helpers;
using System;

namespace NetDream.Tests
{
    [TestClass]
    public class StrTest
    {
        [TestMethod]
        public void TestCompare()
        {
            var content = "PUBLISH_STATUS_DRAFT";
            Assert.IsTrue(content.ToUpper().Equals(content, StringComparison.Ordinal));
        }
    }
}