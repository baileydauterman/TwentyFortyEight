namespace TwentyFortyEight
{
    public static class BoardUtils
    {
        public static void Collapse(this int[] values)
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
    }
}
