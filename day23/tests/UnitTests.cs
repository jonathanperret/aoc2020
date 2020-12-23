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
            Assert.AreEqual("67384529", Part1(
                File.ReadAllText("../../../../cli/example.txt")
            ));
        }

        [Test]
        public void TestMove()
        {
            Assert.AreEqual("289154673", Move("389125467"));
        }

        [Test]
        public void TestMove2()
        {
            Assert.AreEqual("936725841", Move("136792584"));
        }
    }
}
