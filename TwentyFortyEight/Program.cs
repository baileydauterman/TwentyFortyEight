namespace TwentyFortyEight
{

    /*
     Initialize the game with two tiles numbered 2 at random positions.
        Print the board after initializing
        Allow the user to make moves.
        The user will make a move by entering a number.
            0 denotes left
            1 denotes right
            2 denotes top
            3 denotes bottom
        Slide the tiles based on the value, if the slide is possible.
        Add a random tile on the board
        Print the board after the move
        End the game if it is won or lost
        Print "Congratulations" if the game is won
        Print "Game over" if the game is lost
     */
    internal class Program
    {
        static Game _board;

        static void Main(string[] args)
        {
            _board = new Game(4);
            _board.Play();
        }
    }
}
