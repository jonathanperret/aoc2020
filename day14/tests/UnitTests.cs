using NUnit.Framework;
using static Program;
using System.IO;

namespace tests
{
    public class UnitTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.AreEqual(208, solve(
                File.ReadAllLines("../../../../cli/example2.txt")
            ));
        }

    }
}
