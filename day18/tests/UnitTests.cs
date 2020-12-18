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
        public void Digit()
        {
            Assert.AreEqual(5, Compute("5"));
        }

        [Test]
        public void Sum()
        {
            Assert.AreEqual(8, Compute("5 + 3"));
        }

        [Test]
        public void Mul()
        {
            Assert.AreEqual(15, Compute("5 * 3"));
        }

        [Test]
        public void MulSum()
        {
            Assert.AreEqual(17, Compute("5 * 3 + 2"));
        }


        [Test]
        public void SumMul()
        {
            Assert.AreEqual(16, Compute("5 + 3 * 2"));
        }

        [Test]
        public void Paren1()
        {
            Assert.AreEqual(8, Compute("(5 + 3)"));
        }

        [Test]
        public void Paren2()
        {
            Assert.AreEqual(16, Compute("(5 + 3) * 2"));
        }

        [Test]
        public void Paren3()
        {
            Assert.AreEqual(11, Compute("5 + (3 * 2)"));
        }

        [Test]
        public void Test1()
        {
            Assert.AreEqual(71, Compute("1 + 2 * 3 + 4 * 5 + 6"));
            // File.ReadAllLines("../../../../cli/example.txt")
        }
    }
}
