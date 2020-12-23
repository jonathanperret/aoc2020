using NUnit.Framework;
using static Program;
using System.IO;
using System.Linq;

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
            Assert.AreEqual("67384529", Part1(
                File.ReadAllText("../../../../cli/example.txt")
            ));
        }

        [Test]
        public void TestPart2()
        {
            Assert.AreEqual((934001, 159792), Part2(
                File.ReadAllText("../../../../cli/example.txt")
            ));
        }

        [Test]
        public void TestAfter1()
        {
            Assert.AreEqual("45678923", After1(new[] { 0, 4, 3, 1, 5, 6, 7, 8, 9, 2 })); // 2, 3, 1, 4, 5, 6, 7, 8, 9
        }

        [Test]
        public void TestBuildNext()
        {
            CollectionAssert.AreEqual(
                new[] { 0, 4, 3, 1, 5, 6, 7, 8, 9, 2 },
                BuildNext(new[] { 2, 3, 1, 4, 5, 6, 7, 8, 9 })
            );
        }

        [Test]
        public void TestMove()
        {
            var next = BuildNext(new[] { 3, 8, 9, 1, 2, 5, 4, 6, 7 });
            var newCurrent = Move(next, current: 3);
            Assert.AreEqual(2, newCurrent);
            Assert.AreEqual("54673289", After1(next));
        }

        [Test]
        public void TestMove2()
        {
            var next = BuildNext(new[] { 1, 3, 6, 7, 9, 2, 5, 8, 4 });
            var newCurrent = Move(next, current: 1);
            Assert.AreEqual(9, newCurrent);
            CollectionAssert.AreEqual("93672584", After1(next));
        }
    }
}
