namespace TwentyFortyEight
{
    public class Board
    {
        private List<int[]> _board;

        public int Dimension { get; private set; } = 0;

        public int ZeroIndexedDimension => Dimension - 1;

        public int this[Coordinate c]
        {
            get
            {
                return _board[c.X][c.Y];
            }
            set
            {
                _board[c.X][c.Y] = value;
            }
        }

        public Board(int dimension)
        {
            Initialize(dimension);
        }

        public string Write()
        {
            var rows = new List<string>
            {
                string.Empty
            };

            foreach (var row in _board)
            {
                rows.Add(string.Join("  ", row));
            }

            return string.Join("\n", rows).Replace("0", ".");
        }


        public void MoveUp()
        {
            for (var i = 0; i < Dimension; i++)
            {
                if (TryGetColumn(i, out var col))
                {
                    col.Collapse();

                    for (var j = 0; j < Dimension; j++)
                    {
                        _board[j][i] = col[j];
                    }
                }
            }
        }

        public void MoveDown()
        {
            for (var i = 0; i < Dimension; i++)
            {
                if (TryGetColumn(i, out var col))
                {
                    col = col.Reverse().ToArray();
                    col.Collapse();
                    col = col.Reverse().ToArray();

                    for (var j = ZeroIndexedDimension; j > -1; j--)
                    {
                        _board[j][i] = col[j];
                    }
                }
            }
        }

        public void MoveLeft()
        {
            for (var i = 0; i < Dimension; i++)
            {
                var row = _board[i];
                row.Collapse();
                _board[i] = row;
            }
        }

        public void MoveRight()
        {
            for (var i = 0; i < Dimension; i++)
            {
                if (TryGetRow(i, out var row))
                {
                    row = row.Reverse().ToArray();
                    row.Collapse();
                    _board[i] = row.Reverse().ToArray();
                }
            }
        }

        public List<Coordinate> GetEmptySpaces()
        {
            var coords = new List<Coordinate>();

            for (var i = 0; i < Dimension; i++)
            {
                for (var j = 0; j < Dimension; j++)
                {
                    if (_board[i][j] == 0)
                    {
                        coords.Add(new Coordinate
                        {
                            X = i,
                            Y = j
                        });
                    }
                }
            }

            return coords;
        }

        public GameStatus GetGameStatus()
        {
            if (_board.Any(x => x.Any(y => y == 2048)))
            {
                return GameStatus.GameWon;
            }

            // if there are no zeros left, you lost
            if (!HasAvailableMoves())
            {
                return GameStatus.GameOver;
            }

            return GameStatus.InProgress;
        }

        private bool TryGetColumn(int index, out int[] column)
        {
            column = _board.Select(x => x[index]).ToArray();
            return column.Sum() > 0;
        }

        private bool TryGetRow(int index, out int[] row)
        {
            row = _board[index];
            return row.Sum() > 0;
        }


        private void Initialize(int dimension)
        {
            Dimension = dimension;
            _board = new List<int[]>();

            while (dimension-- > 0)
            {
                var row = new int[Dimension];

                for (var i = 0; i < Dimension; i++)
                {
                    row[i] = 0;
                }
                _board.Add(row);
            }
        }

        private bool HasAvailableMoves()
        {
            return CheckRows() || CheckColumns();
        }

        private bool CheckRows()
        {
            foreach (var row in _board)
            {
                row.Collapse();

                if (row.Any(x => x == 0))
                {
                    return true;
                }
            }

            return false;
        }

        private bool CheckColumns()
        {
            for (var i = 0; i < Dimension; i++)
            {
                if (!TryGetColumn(i, out var column))
                {
                    continue;
                }

                column.Collapse();

                if (column.Any(x => x == 0))
                {
                    return true;
                }
            }

            return false;
        }

        private int _largestNumber = 0;
    }

    public class Coordinate
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public enum GameStatus
    {
        InProgress,
        GameOver,
        GameWon
    }
}
