namespace ChessGameApplication.Game.BoardServices;

public class AttackDetectionService
{
    private readonly Board _board;

    public AttackDetectionService(Board board)
    {
        _board = board;
    }

    public bool IsSquareUnderAttack(Position position, PieceColor defenderColor)
    {
        foreach (var piece in _board.GetAllPieces())
        {
            if (piece.Color != defenderColor && piece.GetAvailableMoves(_board).Contains(position))
            {
                return true;
            }
        }
        return false;
    }

    public bool IsInCheck(PieceColor kingColor)
    {
        var kingPos = _board.FindKingPosition(kingColor);
        return IsSquareUnderAttack(kingPos, kingColor);
    }
}
