using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Tests
{
    [TestClass]
    public class PathTest
    {
        [TestMethod]
        public void TestCombine()
        {
            var path = "a/../b";
            Assert.AreEqual(Path.GetFullPath(Path.Combine("/", path)), "b");
        }
    }
}
