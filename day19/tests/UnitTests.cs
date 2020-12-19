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

        // [Test]
        public void TestPart1()
        {
            Assert.AreEqual(2, Solve(
                File.ReadAllLines("../../../../cli/example1.txt")
            ));
        }

        [Test]
        public void TestPart2()
        {
            Assert.AreEqual(12, Solve(
                File.ReadAllLines("../../../../cli/example2.txt")
            ));
        }
    }
}
