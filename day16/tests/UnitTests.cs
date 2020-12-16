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
            Assert.AreEqual(11 * 13, Solve(
                File.ReadAllLines("../../../../cli/example.txt")
            ));
        }
    }
}
