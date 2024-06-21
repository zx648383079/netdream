using NetDream.Shared.Helpers;

namespace NetDream.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var content = "PUBLISH_STATUS_DRAFT";
            Assert.IsTrue(content.ToUpper().Equals(content, StringComparison.Ordinal));
        }
    }
}