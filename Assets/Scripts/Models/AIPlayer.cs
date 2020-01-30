using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIPlayer : Player {

    private readonly Difficulty _difficulty;

    private struct Move {
        public int Row { get; set; }
        public int Column { get; set; }
        public int Score { get; set; }
    }

    public AIPlayer(Difficulty difficulty) {
        _difficulty = difficulty;

    }

    public override void MakePlay(Board board) {
        float randomMoveChance = _difficulty == Difficulty.Easy ? 0.6f : _difficulty == Difficulty.Medium ? 0.4f : 0.0f;

        if (Random.value < randomMoveChance) {
            List<int[]> availableMoves = board.GetAvailableMoves();
            int[] move = availableMoves[Random.Range(0, availableMoves.Count)];
            _boardController.MakePlay(move[0], move[1]);

        } else {
            Move move = Minimax(board);
            _boardController.MakePlay(move.Row, move.Column);

        }
    }

    private Move Minimax(Board board, bool max = true) {
        if (board.IsWinner(Marker)) {
            return new Move { Score = 1 };

        } else if (board.IsWinner(GetOpponentMarker())) {
            return new Move { Score = -1 };

        } else if (!board.HasAvailableMoves()) {
            return new Move { Score = 0 };

        }

        char currentMarker = max ? Marker : GetOpponentMarker();

        List<Move> moves = new List<Move>();

        List<int[]> availableMoves = board.GetAvailableMoves();
        foreach (int[] position in availableMoves) {
            int row = position[0];
            int column = position[1];

            Board newBoard = board.Clone();
            newBoard.MakePlay(row, column, currentMarker);

            Move nextMove = Minimax(newBoard, !max);
            nextMove.Row = row;
            nextMove.Column = column;
            moves.Add(nextMove);
        }

        if (max) {
            moves = moves.OrderByDescending(move => move.Score).ToList();

        } else {
            moves = moves.OrderBy(move => move.Score).ToList();

        }

        List<Move> bestMoves = moves.FindAll(move => move.Score == moves[0].Score);
        return bestMoves[Random.Range(0, bestMoves.Count)];
    }
}