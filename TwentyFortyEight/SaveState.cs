namespace TwentyFortyEight
{
    public class SaveState
    {
        public SaveState(IList<int[]> board)
        {
            Timestamp = DateTime.Now;
            Board = board;
        }

        public DateTime Timestamp { get; set; }

        public IList<int[]> Board { get; set; }
    }
}
