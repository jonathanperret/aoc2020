using NUnit.Framework;
using static Program;
using System.IO;
using static Util;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;

namespace tests
{
    public class UnitTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ScoreTest()
        {
            Assert.AreEqual(306, Score(
                (new[] { 3, 2, 10, 6, 8, 5, 9, 4, 7, 1 }).Select(i => (char)i).ToDelimitedString("")
            ));
        }

        [Test]
        public void TestPart2()
        {
            Assert.AreEqual(291, Part2(
                File.ReadAllText("../../../../cli/example.txt")
            ));
        }

        [Test]
        public void TestPart2Full()
        {
            Assert.AreEqual(33266, Part2(
                File.ReadAllText("../../../../cli/input.txt")
            ));
        }

        class MyHashable
        {
            public int value;

            public override bool Equals(object obj)
            {
                if (obj == null || GetType() != obj.GetType())
                {
                    return false;
                }
                MyHashable other = obj as MyHashable;
                W($"Comparing {value} to {other.value}");
                return value == other.value;
            }

            // override object.GetHashCode
            public override int GetHashCode()
            {
                int hash = 0;
                W($"Hashed {value} to {hash}");
                return hash;
            }
        }

        [Test]
        public void HashSetTest()
        {
            var s = new HashSet<MyHashable>();
            var h1 = new MyHashable { value = 1 };
            var h2 = new MyHashable { value = 2 };
            s.Add(h1);
            Assert.True(s.Add(h2));
        }
    }
}
