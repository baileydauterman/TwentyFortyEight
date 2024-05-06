namespace TwentyFortyEight.Console
{
    internal class Program
    {
        private static Game? _game;

        static void Main(string[] args)
        {
            if (int.TryParse(args[0], out var dimension))
            {
                _game = new Game(dimension);
            }
            else
            {
                _game = new Game(4);

            }

            _game.Play();
        }
    }
}
