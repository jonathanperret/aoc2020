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
            Assert.AreEqual(1, Solve(new int[] { 1, 3, 2 }));
            Assert.AreEqual(10, Solve(new int[] { 2, 1, 3 }));
            Assert.AreEqual(27, Solve(new int[] { 1, 2, 3 }));
            Assert.AreEqual(78, Solve(new int[] { 2, 3, 1 }));
            Assert.AreEqual(438, Solve(new int[] { 3, 2, 1 }));
            Assert.AreEqual(1836, Solve(new int[] { 3, 1, 2 }));
        }

        [Test]
        public void Test2()
        {
            Assert.AreEqual(2578, Solve(new int[] { 1, 3, 2 }, 30000000));
        }
    }
}
