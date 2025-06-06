using ChessGameApplication.Game.Figures;

namespace ChessGameApplication.Game.BoardServices;

public class CastlingService
{
    private readonly Board _board;

    public CastlingService(Board board)
    {
        _board = board;
    }

    public bool IsCastling(Position from, Position to) =>
        Math.Abs(from.Row - to.Row) == 2 && from.Column == to.Column;

    public void HandleCastling(King king, Position from, Position to)
    {
        var rookFrom = to.Row == 0 ? new Position(0, from.Column) : new Position(7, from.Column);
        var rookTo = to.Row == 0 ? new Position(3, from.Column) : new Position(5, from.Column);

        ExecuteCastling(king, from, to, rookFrom, rookTo);
    }

    private void ExecuteCastling(King king, Position from, Position to, Position rookFrom, Position rookTo)
    {
        _board.SetPieceAt(to, king);
        _board.SetPieceAt(from, null);
        king.SetPosition(to);
        king.HasMoved = true;

        var rook = _board.GetPieceAt(rookFrom);
        if (rook != null)
        {
            _board.SetPieceAt(rookTo, rook);
            _board.SetPieceAt(rookFrom, null);
            rook.SetPosition(rookTo);
            rook.HasMoved = true;
        }
    }

}
