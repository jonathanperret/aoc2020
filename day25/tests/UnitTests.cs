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
            Assert.AreEqual(14897079, Part1(
                5764801, 17807724
            ));
        }
    }
}
