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
            var (product, placement) = Part1(
                File.ReadAllLines("../../../../cli/example.txt")
            );
            Assert.AreEqual(20899048083289, product);
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

        private static readonly string[] expectedImage = new string[] {
            ".####...#####..#...###..",
            "#####..#..#.#.####..#.#.",
            ".#.#...#.###...#.##.##..",
            "#.#.##.###.#.##.##.#####",
            "..##.###.####..#.####.##",
            "...#.#..##.##...#..#..##",
            "#.##.#..#.#..#..##.#.#..",
            ".###.##.....#...###.#...",
            "#.####.#.#....##.#..#.#.",
            "##...#..#....#..#...####",
            "..#.##...###..#.#####..#",
            "....#.##.#.#####....#...",
            "..##.##.###.....#.##..#.",
            "#...#...###..####....##.",
            ".#.##...#.##.#.#.###...#",
            "#.###.#..####...##..#...",
            "#.###...#.##...#.######.",
            ".###.###.#######..#####.",
            "..##.#..#..#.#######.###",
            "#.#..##.########..#..##.",
            "#.#####..#.#...##..#....",
            "#....##..#.#########..##",
            "#...#.....#..##...###.##",
            "#..###....##.#...##.##.#",
        };

        private static readonly (int, int)[] knownSolution = new[] {
            (1951,1),(2729,1),(2971,1),
            (2311,1),(1427,1),(1489,1),
            (3079,5),(2473,2),(1171,3),
        };

        [Test]
        public void TestMakeImage()
        {
            var image = MakeImage(
                File.ReadAllLines("../../../../cli/example.txt"),
                3,
                knownSolution
            );
            CollectionAssert.AreEqual(expectedImage, image);
        }

        [Test]
        public void TestPart2()
        {
            var (positions, roughness) = Part2(
                File.ReadAllLines("../../../../cli/example.txt"),
                3,
                new[] {
                    (1951,1),(2729,1),(2971,1),
                    (2311,1),(1427,1),(1489,1),
                    (3079,5),(2473,2),(1171,3),
                }
            );
            CollectionAssert.AreEqual(new[] { (2, 2), (1, 16) }, positions);
            Assert.AreEqual(273, roughness);
        }

    }
}
