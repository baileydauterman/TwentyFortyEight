namespace TwentyFortyEight
{
    public class Board
    {
        private List<int[]> _board;

        public int Dimension { get; private set; } = 0;

        public int HighestNumber => CurrentHighest();

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

        public Board(int dimension, int randomStartingLocations = 2)
        {
            _board = new List<int[]>();
            Initialize(dimension, randomStartingLocations);
        }

        /// <summary>
        /// Enumerates over the board from right to left top row to bottom row
        /// </summary>
        /// <returns></returns>
        public IEnumerable<int> Enumerate()
        {
            for (var i = 0; i< Dimension; i++)
            {
                for (var j = 0; j< Dimension; j++)
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
            switch (move)
            {
                case BoardMove.Up:
                    MoveUp();
                    break;

                case BoardMove.Down:
                    MoveDown();
                    break;

                case BoardMove.Left:
                    MoveLeft();
                    break;

                case BoardMove.Right:
                    MoveRight();
                    break;
            }
        }

        public void MoveUp()
        {
            HasStateChanged = false;

            for (var i = 0; i < Dimension; i++)
            {
                if (TryGetColumn(i, out var col))
                {
                    var copy = new int[col.Length];
                    col.CopyTo(copy, 0);
                    col.Collapse();

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

            SetRandomLocation();
        }

        public void MoveDown()
        {
            HasStateChanged = false;

            for (var i = 0; i < Dimension; i++)
            {
                if (TryGetColumn(i, out var col))
                {
                    var copy = new int[col.Length];
                    col.CopyTo(copy, 0);
                    col = col.Reverse().ToArray();
                    col.Collapse(true);
                    col = col.Reverse().ToArray();

                    if (copy.SequenceEqual(col))
                    {
                        continue;
                    }

                    for (var j = Dimension - 1; j > -1; j--)
                    {
                        _board[j][i] = col[j];
                    }

                    HasStateChanged = true;
                }
            }

            SetRandomLocation();
        }

        public void MoveLeft()
        {
            HasStateChanged = false;

            for (var i = 0; i < Dimension; i++)
            {
                if (TryGetRow(i, out var row))
                {
                    var copy = row.Clone() as int[];
                    row.Collapse();

                    if (copy is not null && row.SequenceEqual(copy))
                    {
                        continue;
                    }

                    _board[i] = row;
                    HasStateChanged = true;
                }
            }

            SetRandomLocation();
        }

        public void MoveRight()
        {
            HasStateChanged = false;
            for (var i = 0; i < Dimension; i++)
            {
                if (TryGetRow(i, out var row))
                {
                    var copy = row.Clone() as int[];
                    row = row.Reverse().ToArray();
                    row.Collapse();
                    row = row.Reverse().ToArray();

                    if (copy is not null && row.SequenceEqual(copy))
                    {
                        continue;
                    }

                    _board[i] = row;
                    HasStateChanged = true;
                }
            }

            SetRandomLocation();
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

            var randCoord = Random.Shared.Next(coords.Count);
            var location = coords[randCoord];
            var flip = Random.Shared.Next(0, 2);

            this[location] = flip > 0 ? 2 : 4;
        }

        private bool ReachedTarget()
        {
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
                copy.Collapse();

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
                copy.Collapse();


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
