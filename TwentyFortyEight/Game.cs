namespace TwentyFortyEight
{
    public class Game
    {
        public Board Board;

        public Game(int dimension)
        {
            Board = new Board(dimension);
        }

        public void Play()
        {
            while (true)
            {
                WriteBoard();
                switch (Board.GameStatus)
                {
                    case GameStatus.InProgress:
                        WaitKeyPress();
                        break;

                    case GameStatus.GameWon:
                        Console.WriteLine($"Congratulations you made it to {Board.Target}");
                        var keepPlaying = WaitYesNo("Would you like to keep playing? (y/n)");

                        if (keepPlaying)
                        {
                            Board.Target = Board.Target << 1; // double the value of previous target
                        }
                        else
                        {
                            Console.WriteLine("Thanks for playing!");
                            return;
                        }
                        break;

                    case GameStatus.GameOver:
                        Console.WriteLine("Looks like you're out of moves.");
                        var tryAgain = WaitYesNo("Try again? (y/n)");

                        if (tryAgain)
                        {
                            Board = new Board(Board.Dimension);
                        }
                        else
                        {
                            Console.WriteLine("Thanks for playing!");
                            return;
                        }
                        break;
                }
            }
        }

        public void WaitKeyPress()
        {
            if (Board.HasStateChanged)
            {
                Console.WriteLine("Use arrow keys to move");
            }

            while (!HandleArrowKey())
            {

            }
        }

        public bool WaitYesNo(string prompt)
        {
            while (true)
            {
                Console.WriteLine(prompt);
                var response = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(response))
                {
                    continue;
                }

                switch (response.ToLower())
                {
                    case "y":
                    case "yes":
                        return true;
                    case "n":
                    case "no":
                        return false;
                    default:
                        break;
                }
            }
        }


        public bool HandleArrowKey()
        {
            var readKey = Console.ReadKey();

            switch (readKey.Key)
            {
                case ConsoleKey.UpArrow:
                    Board.MoveUp();
                    break;

                case ConsoleKey.DownArrow:
                    Board.MoveDown();
                    break;

                case ConsoleKey.LeftArrow:
                    Board.MoveLeft();
                    break;

                case ConsoleKey.RightArrow:
                    Board.MoveRight();
                    break;

                default:
                    return false;
            }

            return true;
        }

        private void WriteBoard()
        {
            if (Board.HasStateChanged)
            {
                Console.Clear();
                Console.WriteLine(Board.Write());
            }
        }
    }
}
