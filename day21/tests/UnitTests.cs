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
        public void TestSolve()
        {
            Assert.AreEqual((5, "mxmxvkd,sqjhc,fvjkl"), Solve(
                File.ReadAllLines("../../../../cli/example.txt")
            ));
        }
    }
}
