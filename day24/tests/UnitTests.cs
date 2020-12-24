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
            ).Length);
        }

        [Test]
        public void TestPart2_1()
        {
            Assert.AreEqual(15, Part2BitSumsNoAlloc(
                Part1(File.ReadAllLines("../../../../cli/example.txt")),
                1
            ));
        }

        [Test]
        public void TestPart2()
        {
            Assert.AreEqual(2208, Part2BitSumsNoAlloc(
                Part1(File.ReadAllLines("../../../../cli/example.txt")),
                100
            ));
        }
    }
}
