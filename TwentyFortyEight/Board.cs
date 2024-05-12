namespace TwentyFortyEight
{
    public class Board
    {
        private List<int[]> _board;

        public int Dimension { get; private set; } = 0;

        public int HighestNumber => CurrentHighest();

        public int Score = 0;

        public int Target
        {
            get
            {
                return _target;
            }
            set
            {
                HasStateChanged = true;
                _target = value;
            }
        }

        private int _target = 2048;

        private readonly Random _random = Random.Shared;

        public GameStatus GameStatus => GetGameStatus();

        public bool HasStateChanged { get; set; } = true;

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

        public Board(SaveState state)
        {
            _board = state.Board;
            Score = state.Score;
            Dimension = _board.Count;
        }

        public Board(int dimension, int randomStartingLocations = 2, int? seed = null)
        {
            if (seed is not null)
            {
                _random = new Random(seed.Value);
            }

            _board = new List<int[]>();
            Initialize(dimension, randomStartingLocations);
        }

        /// <summary>
        /// Enumerates over the board from right to left top row to bottom row
        /// </summary>
        /// <returns></returns>
        public IEnumerable<int> Enumerate()
        {
            for (var i = 0; i < Dimension; i++)
            {
                for (var j = 0; j < Dimension; j++)
                {
                    var coord = new Coordinate
                    {
                        X = i,
                        Y = j,
                    };

                    yield return this[coord];
                }
            }
        }

        public IEnumerable<BoardCell> EnumerateCells()
        {
            for (var i = 0; i < Dimension; i++)
            {
                for (var j = 0; j < Dimension; j++)
                {
                    var coord = new Coordinate
                    {
                        X = i,
                        Y = j,
                    };

                    yield return new BoardCell
                    {
                        Coordinate = coord,
                        Value = this[coord]
                    };
                }
            }
        }

        public IEnumerable<BoardCell> GetRow(int i)
        {
            var row = _board[i];
            var y = 0;
            foreach (var cell in row)
            {
                yield return new BoardCell
                {
                    Coordinate = new Coordinate
                    {
                        X = i,
                        Y = y++,
                    },
                    Value = cell,
                };
            }
        }

        public string Write()
        {
            var rows = new List<string>
            {
                "Current Highest:\t\tTarget:",
                $"{CurrentHighest()}\t\t\t\t{Target}",
                string.Empty,
                this.ToString(),
            };

            return string.Join("\n", rows);
        }

        public override string ToString()
        {
            var rows = new List<string>();
            foreach (var row in _board)
            {
                var str = string.Empty;
                foreach (var value in row)
                {
                    if (value == 0)
                    {
                        str += $".  ";
                    }
                    else
                    {
                        str += $"{value}  ";
                    }
                }

                rows.Add(str);
            }

            return string.Join("\n", rows);
        }

        #region Movements
        public void Move(BoardMove move)
        {
            HasStateChanged = false;

            switch (move)
            {
                case BoardMove.Up:
                    MoveColumn();
                    break;

                case BoardMove.Down:
                    MoveColumn(true);
                    break;

                case BoardMove.Left:
                    MoveRow();
                    break;

                case BoardMove.Right:
                    MoveRow(true);
                    break;
            }

            SetRandomLocation();
        }

        private void MoveColumn(bool reverse = false)
        {
            for (var i = 0; i < Dimension; i++)
            {
                if (TryGetColumn(i, out var col))
                {
                    var copy = new int[col.Length];
                    col.CopyTo(copy, 0);

                    if (reverse)
                    {
                        col = col.Reverse().ToArray();
                        Collapse(col);
                        col = col.Reverse().ToArray();
                    }
                    else
                    {
                        Collapse(col);
                    }

                    Score += col.Sum() - copy.Sum();

                    if (copy.SequenceEqual(col))
                    {
                        continue;
                    }

                    for (var j = 0; j < Dimension; j++)
                    {
                        _board[j][i] = col[j];
                    }

                    HasStateChanged = true;
                }
            }
        }

        private void MoveRow(bool reverse = false)
        {
            for (var i = 0; i < Dimension; i++)
            {
                if (TryGetRow(i, out var row))
                {
                    var copy = new int[row.Length];
                    row.CopyTo(copy, 0);

                    if (reverse)
                    {
                        row = row.Reverse().ToArray();
                        Collapse(row);
                        row = row.Reverse().ToArray();
                    }
                    else
                    {
                        Collapse(row);
                    }

                    Score += row.Sum() - copy.Sum();

                    if (row.SequenceEqual(copy))
                    {
                        continue;
                    }

                    _board[i] = row;
                    HasStateChanged = true;
                }
            }
        }

        public void Collapse(int[] values)
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
                    var newValue = originaValue + nextValue;
                    values[origin] = newValue;
                    values[next] = 0;
                    Score += newValue;
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

        #endregion

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
            if (ReachedTarget())
            {
                return GameStatus.GameWon;
            }

            if (!HasAvailableSpace())
            {
                return GameStatus.GameOver;
            }

            return GameStatus.InProgress;
        }

        public SaveState Save()
        {
            return new SaveState(_board, Score);
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

        private void Initialize(int dimension, int startLocationCount)
        {
            Dimension = dimension;

            while (dimension-- > 0)
            {
                var row = new int[Dimension];

                for (var i = 0; i < Dimension; i++)
                {
                    row[i] = 0;
                }
                _board.Add(row);
            }

            for (var i = 1; i <= startLocationCount; i++)
            {
                SetRandomLocation();
            }
        }

        private void SetRandomLocation()
        {
            if (!HasStateChanged)
            {
                return;
            }

            var coords = GetEmptySpaces();

            if (coords.Count <= 0)
            {
                return;
            }

            var randCoord = _random.Next(coords.Count);
            var location = coords[randCoord];
            var flip = _random.Next(0, 2);

            this[location] = flip > 0 ? 2 : 4;
        }

        private bool ReachedTarget()
        {
            if (Dimension == 0)
            {
                return false;
            }

            return _board.AsParallel().WithDegreeOfParallelism(Dimension).Any(x => x.Any(y => y == Target));
        }

        private bool HasAvailableSpace()
        {
            return CheckRows() || CheckColumns();
        }

        private bool CheckRows()
        {
            foreach (var row in _board)
            {
                var copy = new int[row.Length];
                row.CopyTo(copy, 0);
                Collapse(copy);

                if (row.SequenceEqual(copy) || copy.Any(x => x == 0))
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

                var copy = new int[column.Length];
                column.CopyTo(copy, 0);
                Collapse(copy);


                if (copy.SequenceEqual(column) || copy.Any(x => x == 0))
                {
                    return true;
                }
            }

            return false;
        }

        private int CurrentHighest()
        {
            var max = 0;
            foreach (var r in _board)
            {
                var temp = r.Max();
                max = temp > max ? temp : max;
            }

            return max;
        }
    }

    public class BoardCell
    {
        public Coordinate Coordinate { get; set; } = new Coordinate();

        public int Value { get; set; }
    }
}
