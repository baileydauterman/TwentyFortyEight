namespace TwentyFortyEight.Console
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
                        System.Console.WriteLine($"Congratulations you made it to {Board.Target}");
                        var keepPlaying = WaitYesNo("Would you like to keep playing? (y/n)");

                        if (keepPlaying)
                        {
                            Board.Target = Board.Target << 1; // double the value of previous target
                        }
                        else
                        {
                            System.Console.WriteLine("Thanks for playing!");
                            return;
                        }
                        break;

                    case GameStatus.GameOver:
                        System.Console.WriteLine("Looks like you're out of moves.");
                        var tryAgain = WaitYesNo("Try again? (y/n)");

                        if (tryAgain)
                        {
                            Board = new Board(Board.Dimension);
                        }
                        else
                        {
                            System.Console.WriteLine("Thanks for playing!");
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
                System.Console.WriteLine("Use arrow keys to move");
            }

            while (!HandleArrowKey())
            {

            }
        }

        public bool WaitYesNo(string prompt)
        {
            while (true)
            {
                System.Console.WriteLine(prompt);
                var response = System.Console.ReadLine();

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
            var readKey = System.Console.ReadKey();

            if (readKey.Key.TryConvertConsoleKeyToBoardMove(out var move))
            {
                Board.Move(move);
                return true;
            }

            return false;
        }

        private void WriteBoard()
        {
            if (Board.HasStateChanged)
            {
                System.Console.Clear();
                System.Console.WriteLine(Board.Write());
            }
        }
    }
}
