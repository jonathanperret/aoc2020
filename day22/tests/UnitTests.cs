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
        public void ScoreTest()
        {
            Assert.AreEqual(306, Score(
                new long[] { 3, 2, 10, 6, 8, 5, 9, 4, 7, 1 }
            ));
        }

        [Test]
        public void TestPart2()
        {
            Assert.AreEqual(291, Part2(
                File.ReadAllText("../../../../cli/example.txt")
            ));
        }
    }
}
