namespace TwentyFortyEight.Tests
{
    public class Tests
    {
        [TestCase(new int[] { 0, 2, 0, 2 }, new int[] { 4, 0, 0, 0 })]
        [TestCase(new int[] { 2, 2, 0, 2 }, new int[] { 4, 2, 0, 0 })]
        [TestCase(new int[] { 2, 2, 2, 2 }, new int[] { 4, 4, 0, 0 })]
        [TestCase(new int[] { 2, 2, 4, 2 }, new int[] { 4, 4, 2, 0 })]
        [TestCase(new int[] { 8, 4, 4, 2 }, new int[] { 8, 8, 2, 0 })]
        [TestCase(new int[] { 8, 4, 2, 4 }, new int[] { 8, 4, 2, 4 })]
        public void Collapse(int[] value, int[] expected)
        {
            value.Collapse();
            Assert.That(value, Is.EqualTo(expected));
        }
    }
}