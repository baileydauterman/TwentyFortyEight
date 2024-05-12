namespace TwentyFortyEight
{
    public class SaveState
    {
        public SaveState(List<int[]> board, int score)
        {
            Timestamp = DateTime.Now;
            Board = board;
            Score = score;
        }

        public DateTime Timestamp { get; set; }

        public List<int[]> Board { get; set; }

        public int Score { get; set; }
    }
}
