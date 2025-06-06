using ChessGameApplication.Game.Figures;

namespace ChessGameApplication.Game.BoardServices;

public class BoardValidationService
{
    private readonly Board _board;

    public BoardValidationService(Board board)
    {
        _board = board;
    }

    public bool IsEmpty(Position pos) => _board.GetPieceAt(pos) == null;

    public bool IsEnemyPiece(Position pos, PieceColor color) =>
        _board.GetPieceAt(pos) is Piece piece && piece.Color != color;

    public bool IsInsideBoard(Position pos) =>
        pos.Row >= 0 && pos.Row < 8 && pos.Column >= 0 && pos.Column < 8;
}
