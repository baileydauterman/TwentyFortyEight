using System.Text.Json;

namespace TwentyFortyEight.Console
{
    public class Game
    {
        public Board Board;

        public Game(int dimension)
        {
            localAppData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "2048");
            saveStatePath = Path.Combine(localAppData, "saveState.json");

            if (File.Exists(saveStatePath) && TryReadSaveState(out var state))
            {
                Board = new Board(state);
            }
            else
            {
                Board = new Board(dimension);
            }
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
                WriteSaveState();
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

        private void WriteSaveState()
        {
            Directory.CreateDirectory(localAppData);
            var serialized = JsonSerializer.Serialize(state);

            using (var file = File.OpenWrite(saveStatePath))
            {
                file.SetLength(0);
            }

            File.WriteAllText(saveStatePath, serialized);
        }

        private bool TryReadSaveState(out SaveState? state)
        {
            state = null;
            using (var stream = File.OpenRead(saveStatePath))
            {
                try
                {
                    state = JsonSerializer.Deserialize<SaveState>(stream);
                }
                catch
                {

                }
            }

            return state is not null;
        }

        private SaveState state => Board.Save();
        private readonly string localAppData;
        private readonly string saveStatePath;
    }
}
