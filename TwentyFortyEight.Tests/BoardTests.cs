using System.Text.Json;

namespace TwentyFortyEight.Tests
{
    public class BoardTests
    {
        [TestCase(new int[] { 0, 2, 0, 2 }, new int[] { 4, 0, 0, 0 })]
        [TestCase(new int[] { 2, 2, 0, 2 }, new int[] { 4, 2, 0, 0 })]
        [TestCase(new int[] { 2, 2, 2, 2 }, new int[] { 4, 4, 0, 0 })]
        [TestCase(new int[] { 2, 2, 4, 2 }, new int[] { 4, 4, 2, 0 })]
        [TestCase(new int[] { 8, 4, 4, 2 }, new int[] { 8, 8, 2, 0 })]
        [TestCase(new int[] { 8, 4, 2, 4 }, new int[] { 8, 4, 2, 4 })]
        public void Collapse(int[] value, int[] expected)
        {
            var board = new Board(4);
            board.Collapse(value);
            Assert.That(value, Is.EqualTo(expected));
        }

        [Test]
        public void SaveState()
        {
            var board = new Board(4, seed: 1);

            // make x circular passes to sufficiently change board
            for (var i = 0; i < board.Dimension; i++)
            {
                board.Move(BoardMove.Right);
                board.Move(BoardMove.Down);
                board.Move(BoardMove.Left);
                board.Move(BoardMove.Up);
            }

            var state = board.Save();
            state.Timestamp = DateTime.MinValue;
            var serialized = JsonSerializer.Serialize(state);
            var expected = TestingResources.BasicSaveStateExpected;

            // This ensures that save state and random seed are working properly
            Assert.That(serialized, Is.EqualTo(expected));
        }
    }
}