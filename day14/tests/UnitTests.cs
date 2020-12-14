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
            Assert.AreEqual(208, Program.solve(
                File.ReadAllLines("/Users/jonathanperret/Documents/projects/aoc2020/day14/cli/example2.txt")
            ));
        }

    }
}
