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

        internal static void Collapse(this int[] values, bool reverse = false)
        {
            var dimension = values.Length;
            var origin = 0;
            var next = 0;

            while (origin <= dimension - 1 && next < dimension)
            {
                if (origin + 1 == values.Length)
                {
                    break;
                }

                next = GetNextValueIndex(values, origin + 1);

                var originaValue = values[origin];
                var nextValue = values[next];

                if (next == dimension - 1 && nextValue == 0)
                {
                    break;
                }

                if (originaValue == nextValue)
                {
                    values[origin] = originaValue + nextValue;
                    values[next] = 0;
                    origin++;
                }
                else if (originaValue == 0)
                {
                    values[origin] = nextValue;
                    values[next] = 0;
                }
                else
                {
                    origin++;
                }
            }
        }

        private static int GetNextValueIndex(int[] values, int startIndex)
        {
            if (startIndex == values.Length)
            {
                return startIndex - 1;
            }
            else if (startIndex == values.Length - 1)
            {
                return startIndex;
            }

            var len = values.Length - 1;

            while (startIndex < len && values[startIndex] == 0)
            {
                startIndex++;
            }

            return startIndex;
        }

        private static readonly Dictionary<string, BoardMove> KeyToBoardMove = new Dictionary<string, BoardMove>()
        {
            { "ArrowUp", BoardMove.Up },
            { "ArrowDown", BoardMove.Down },
            { "ArrowRight", BoardMove.Right },
            { "ArrowLeft", BoardMove.Left }
        };

        private static readonly Dictionary<ConsoleKey, BoardMove> ConsoleKeyToBoardMove = new Dictionary<ConsoleKey, BoardMove>()
        {
            { ConsoleKey.UpArrow, BoardMove.Up },
            { ConsoleKey.DownArrow, BoardMove.Down },
            { ConsoleKey.LeftArrow, BoardMove.Left },
            { ConsoleKey.RightArrow, BoardMove.Right },
        };
    }
}
