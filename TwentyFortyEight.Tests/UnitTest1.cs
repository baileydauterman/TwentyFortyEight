namespace TwentyFortyEight.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            Board = new Board(4);
        }

        [TestCase(new int[] { 0, 2, 0, 2 }, new int[] { 4, 0, 0, 0 })]
        [TestCase(new int[] { 2, 2, 0, 2 }, new int[] { 4, 2, 0, 0 })]
        [TestCase(new int[] { 2, 2, 2, 2 }, new int[] { 4, 4, 0, 0 })]
        [TestCase(new int[] { 2, 2, 4, 2 }, new int[] { 4, 4, 2, 0 })]
        [TestCase(new int[] { 8, 4, 4, 2 }, new int[] { 8, 8, 2, 0 })]
        [TestCase(new int[] { 8, 4, 2, 4 }, new int[] { 8, 4, 2, 4 })]
        public void Collapse(int[] value, int[] expected)
        {
            Board.Collapse(value);
            Assert.That(value, Is.EqualTo(expected));
        }

        private Board Board { get; set; }
    }
}