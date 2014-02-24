using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConsoleTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string str = string.Format("{{{0}}}", 1);
            Console.WriteLine(str);
        }

        
    }
}
