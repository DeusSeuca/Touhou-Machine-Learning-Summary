using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace 单元测试
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Command.RowCommand.GetCardList("");
        }
    }
}
