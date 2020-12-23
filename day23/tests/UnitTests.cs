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

        [Test]
        public void TestPart1()
        {
            Assert.AreEqual("67384529", Part1(
                File.ReadAllText("../../../../cli/example.txt")
            ));
        }

        [Test]
        public void TestAfter1()
        {
            Assert.AreEqual("45678923", After1(new[] { 2, 3, 1, 4, 5, 6, 7, 8, 9 }));
            Assert.AreEqual("23456789", After1(new[] { 2, 3, 4, 5, 6, 7, 8, 9, 1 }));
        }

        [Test]
        public void TestMove()
        {
            CollectionAssert.AreEqual(
                new[] { 2, 8, 9, 1, 5, 4, 6, 7, 3 },
                Move(new[] { 3, 8, 9, 1, 2, 5, 4, 6, 7 })
            );
        }

        [Test]
        public void TestMove2()
        {
            CollectionAssert.AreEqual(
                new[] { 9, 3, 6, 7, 2, 5, 8, 4, 1 },
                Move(new[] { 1, 3, 6, 7, 9, 2, 5, 8, 4 })
            );
        }
    }
}
