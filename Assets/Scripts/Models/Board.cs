using System.Collections.Generic;

public class Board {

    private char[,] _board;

    public Board() {
        _board = new char[Constants.Board.SIZE, Constants.Board.SIZE];
    }

    public bool MakePlay(int row, int col, char marker) {
        if (_board[row, col].Equals(default)) {
            _board[row, col] = marker;
            return true;

        }

        return false;
    }

    public List<int[]> GetAvailableMoves() {
        List<int[]> availableMoves = new List<int[]>();

        for (int row = 0; row < Constants.Board.SIZE; row++) {
            for (int col = 0; col < Constants.Board.SIZE; col++) {
                if (_board[row, col].Equals(default)) {
                    availableMoves.Add(new int[2] { row, col });
                }
            }
        }

        return availableMoves;
    }

    public bool HasAvailableMoves() {
        for (int row = 0; row < Constants.Board.SIZE; row++) {
            for (int col = 0; col < Constants.Board.SIZE; col++) {
                if (_board[row, col].Equals(default)) {
                    return true;
                }
            }
        }

        return false;
    }

    public bool IsWinner(char marker) {

        //Rows
        for (int rowIdx = 0; rowIdx < Constants.Board.SIZE; rowIdx++) {
            int counter = 0;

            for (int colIdx = 0; colIdx < Constants.Board.SIZE; colIdx++) {
                char play = GetMarkerAt(rowIdx, colIdx);
                if (!play.Equals(marker)) {
                    break;
                }

                counter++;
            }

            if (counter == Constants.Board.SIZE)
                return true;
        }

        //Columns
        for (int colIdx = 0; colIdx < Constants.Board.SIZE; colIdx++) {
            int counter = 0;

            for (int rowIdx = 0; rowIdx < Constants.Board.SIZE; rowIdx++) {
                char play = GetMarkerAt(rowIdx, colIdx);
                if (!play.Equals(marker)) {
                    break;
                }

                counter++;
            }

            if (counter == Constants.Board.SIZE)
                return true;
        }

        //Diagonal 1
        int diagCounter = 0;

        for (int idx = 0; idx < Constants.Board.SIZE; idx++) {
            char play = GetMarkerAt(idx, idx);
            if (!play.Equals(marker)) {
                break;
            }

            diagCounter++;
        }

        if (diagCounter == Constants.Board.SIZE)
            return true;

        //Diagonal 2
        int diag2Counter = 0;

        for (int rowIdx = 0; rowIdx < Constants.Board.SIZE; rowIdx++) {
            int colIdx = Constants.Board.SIZE - rowIdx - 1;

            char play = GetMarkerAt(rowIdx, colIdx);
            if (!play.Equals(marker)) {
                break;
            }

            diag2Counter++;
        }

        if (diag2Counter == Constants.Board.SIZE)
            return true;

        return false;
    }

    public List<int[]> GetWinningPositions() {
        char marker = default;
        List<int[]> positions = new List<int[]>();

        //Rows
        for (int rowIdx = 0; rowIdx < Constants.Board.SIZE; rowIdx++) {
            marker = default;
            positions = new List<int[]>();

            for (int colIdx = 0; colIdx < Constants.Board.SIZE; colIdx++) {
                char play = GetMarkerAt(rowIdx, colIdx);
                if (play.Equals(default)) {
                    break;
                }

                if (marker.Equals(default)) {
                    marker = play;
                }

                if (marker.Equals(play)) {
                    positions.Add(new int[] { rowIdx, colIdx });

                } else {
                    break;

                }
            }

            if (positions.Count == Constants.Board.SIZE)
                return positions;
        }

        //Columns
        for (int colIdx = 0; colIdx < Constants.Board.SIZE; colIdx++) {
            marker = default;
            positions = new List<int[]>();

            for (int rowIdx = 0; rowIdx < Constants.Board.SIZE; rowIdx++) {
                char play = GetMarkerAt(rowIdx, colIdx);
                if (play.Equals(default)) {
                    break;
                }

                if (marker.Equals(default)) {
                    marker = play;
                }

                if (marker.Equals(play)) {
                    positions.Add(new int[] { rowIdx, colIdx });

                } else {
                    break;

                }
            }

            if (positions.Count == Constants.Board.SIZE)
                return positions;
        }

        //Diagonal 1
        marker = default;
        positions = new List<int[]>();

        for (int idx = 0; idx < Constants.Board.SIZE; idx++) {
            char play = GetMarkerAt(idx, idx);
            if (play.Equals(default)) {
                break;
            }

            if (marker.Equals(default)) {
                marker = play;
            }

            if (marker.Equals(play)) {
                positions.Add(new int[] { idx, idx });

            } else {
                break;

            }
        }

        if (positions.Count == Constants.Board.SIZE)
            return positions;

        //Diagonal 2
        marker = default;
        positions = new List<int[]>();

        for (int rowIdx = 0; rowIdx < Constants.Board.SIZE; rowIdx++) {
            int colIdx = Constants.Board.SIZE - rowIdx - 1;

            char play = GetMarkerAt(rowIdx, colIdx);
            if (play.Equals(default)) {
                break;
            }

            if (marker.Equals(default)) {
                marker = play;
            }

            if (marker.Equals(play)) {
                positions.Add(new int[] { rowIdx, colIdx });

            } else {
                break;

            }
        }

        if (positions.Count == Constants.Board.SIZE)
            return positions;

        return null;
    }

    public char GetMarkerAt(int row, int col) {
        return _board[row, col];
    }

    public Board Clone() {
        return new Board {
            _board = (char[,])_board.Clone()
        };
    }
}
