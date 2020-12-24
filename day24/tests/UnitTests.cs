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
        public void TestPart1()
        {
            Assert.AreEqual(10, Part1(
                File.ReadAllLines("../../../../cli/example.txt")
            ));
        }
    }
}
