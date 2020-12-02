using NUnit.Framework;
using cli;
using Sprache;

namespace tests
{
    public class Day2Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Day2Test1()
        {
            Assert.AreEqual((1, 2, 'a', "baba"), Program.Line.Parse("1-2 a: baba"));
        }

    }
}