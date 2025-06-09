namespace ChessGameApplication.Game.BoardServices;

public class GameStateService
{
    private readonly Board _board;
    private readonly AttackDetectionService _attackDetectionService;

    public GameStateService(Board board, AttackDetectionService attackDetectionService)
    {
        _board = board;
        _attackDetectionService = attackDetectionService;
    }

    private Dictionary<Position, List<Position>> GetAllPossibleMoves(PieceColor color)
    {
        var allMoves = new Dictionary<Position, List<Position>>();

        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                var piece = _board.GetPieceAt(new Position(row, col));
                if (piece?.Color == color)
                {
                    var position = new Position(row, col);
                    var validMoves = piece.GetAvailableMoves(_board)
                        .Where(move => !_board.WouldBeInCheckAfterMove(position, move, color))
                        .ToList();

                    if (validMoves.Any())
                    {
                        allMoves[position] = validMoves;
                    }
                }
            }
        }

        return allMoves;
    }

    public bool IsCheckmate(PieceColor kingColor) =>
        _attackDetectionService.IsInCheck(kingColor) && !GetAllPossibleMoves(kingColor).Any();

    public bool IsStalemate(PieceColor currentPlayer) =>
        !_attackDetectionService.IsInCheck(currentPlayer) && !GetAllPossibleMoves(currentPlayer).Any();
}
