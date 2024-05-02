namespace TwentyFortyEight
{
    public class Game
    {
        public Board _board;

        public Game(int dimension)
        {
            _board = new Board(dimension);
            SetRandomLocation();
            SetRandomLocation();
        }

        public void Play()
        {
            Console.WriteLine(_board.Write());

            while (_board.GetGameStatus() == GameStatus.InProgress)
            {
                AskKeyMove();
                SetRandomLocation();
                Console.Clear();
                Console.WriteLine(_board.Write());
            }
        }

        public void AskKeyMove()
        {
            Console.WriteLine("Use arrow keys to move");

            while (!ArrowKeyMove())
            {

            }

            SetRandomLocation();
        }


        public bool ArrowKeyMove()
        {
            var readKey = Console.ReadKey();

            switch (readKey.Key)
            {
                case ConsoleKey.UpArrow:
                    _board.MoveUp();
                    break;
                case ConsoleKey.DownArrow:
                    _board.MoveDown();
                    break;
                case ConsoleKey.LeftArrow:
                    _board.MoveLeft();
                    break;
                case ConsoleKey.RightArrow:
                    _board.MoveRight();
                    break;
                default:
                    return false;
            }

            return true;
        }

        private void SetRandomLocation()
        {
            var coords = _board.GetEmptySpaces();

            if (coords.Count <= 0)
            {
                return;
            }

            var randCoord = Random.Shared.Next(coords.Count);
            var location = coords[randCoord];
            var flip = Random.Shared.Next(0, 2);

            _board[location] = flip > 0 ? 2 : 4;
        }
    }
}
