using ChessGameApplication.Game.Figures;

namespace ChessGameApplication.Game.BoardServices;

public class MovementService
{
    private readonly Board _board;

    public MovementService(Board board)
    {
        _board = board;
    }

    public void UpdatePiecePosition(Piece piece, Position from, Position to)
    {
        _board.SetPieceAt(to, piece);
        _board.SetPieceAt(from, null);
        piece.SetPosition(to);
        piece.HasMoved = true;
    }

    public void MovePieceTemporarily(Position from, Position to)
    {
        var piece = _board.GetPieceAt(from);
        if (piece == null) return;

        _board.SetPieceAt(to, piece);
        _board.SetPieceAt(from, null);
        piece.SetPosition(to);
    }

}
