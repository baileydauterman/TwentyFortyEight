namespace TwentyFortyEight
{
    public static class BoardExtensions
    {
        public static bool TryConvertWebKeyToBoardMoveType(this string key, out BoardMove move)
        {
            return KeyToBoardMove.TryGetValue(key, out move);
        }

        public static bool TryConvertConsoleKeyToBoardMove(this ConsoleKey key, out BoardMove move)
        {
            return ConsoleKeyToBoardMove.TryGetValue(key, out move);
        }

        private static readonly Dictionary<string, BoardMove> KeyToBoardMove = new()
        {
            { "ArrowUp", BoardMove.Up },
            { "ArrowDown", BoardMove.Down },
            { "ArrowRight", BoardMove.Right },
            { "ArrowLeft", BoardMove.Left }
        };

        private static readonly Dictionary<ConsoleKey, BoardMove> ConsoleKeyToBoardMove = new()
        {
            { ConsoleKey.UpArrow, BoardMove.Up },
            { ConsoleKey.DownArrow, BoardMove.Down },
            { ConsoleKey.LeftArrow, BoardMove.Left },
            { ConsoleKey.RightArrow, BoardMove.Right },
        };
    }
}
