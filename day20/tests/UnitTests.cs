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
            var solution = Solve(
                File.ReadAllLines("../../../../cli/example.txt")
            );
            Assert.AreEqual(20899048083289, solution);
        }

        [Test]
        public void TestRotate()
        {
            string[] rotated = Rotate(new[] {
                "AB",
                "CD",
            });
            Assert.AreEqual("CA", rotated[0]);
            Assert.AreEqual("DB", rotated[1]);
        }

        [Test]
        public void TestFlip()
        {
            string[] flipped = Flip(new[] {
                "AB",
                "CD",
            });
            Assert.AreEqual("BA", flipped[0]);
            Assert.AreEqual("DC", flipped[1]);
        }

        [Test]
        public void TestMatchRight()
        {
            string[] left = new[] {
                "AB",
                "CD",
            };
            string[] right = new[] {
                "BE",
                "DF",
            };
            Assert.IsTrue(MatchRight(left, right));
            Assert.IsFalse(MatchRight(right, left));
        }

        [Test]
        public void TestMatchBottom()
        {
            string[] top = new[] {
                "AB",
                "CD",
            };
            string[] bottom = new[] {
                "CD",
                "EF",
            };
            Assert.IsTrue(MatchBottom(top, bottom));
            Assert.IsFalse(MatchBottom(bottom, top));
        }
    }
}
