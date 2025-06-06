using ChessGameApplication.Game.Figures;

namespace ChessGameApplication.Game.BoardServices;

public class PiecePlacementService
{
    private const int BoardSize = 8;
    private readonly Board _board;

    public PiecePlacementService(Board board)
    {
        _board = board;
    }

    public void SetStartingPieces()
    {
        var startingPieces = new (Type pieceType, int x, int y)[]
        {
            (typeof(Rook), 0, 0), (typeof(Knight), 1, 0), (typeof(Bishop), 2, 0),
            (typeof(Queen), 3, 0), (typeof(King), 4, 0),
            (typeof(Bishop), 5, 0), (typeof(Knight), 6, 0), (typeof(Rook), 7, 0)
        };

        foreach (var (type, x, y) in startingPieces)
            _board.PlacePiece((Piece)Activator.CreateInstance(type, PieceColor.Black, new Position(x, y))!, new Position(x, y));

        foreach (var (type, x, y) in startingPieces)
            _board.PlacePiece((Piece)Activator.CreateInstance(type, PieceColor.White, new Position(x, 7))!, new Position(x, 7));

        for (int x = 0; x < BoardSize; x++)
        {
            _board.PlacePiece(new Pawn(PieceColor.Black, new Position(x, 1)), new Position(x, 1));
            _board.PlacePiece(new Pawn(PieceColor.White, new Position(x, 6)), new Position(x, 6));
        }
    }

    public void ClearBoard()
    {
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                _board.SetPieceAt(new Position(row, col), null);
            }
        }
    }

    public void PlacePiece(Piece? piece, Position position)
    {
        _board.SetPieceAt(position, piece);
        piece?.SetPosition(position);
    }

}
