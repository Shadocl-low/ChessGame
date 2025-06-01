// ChessGameApplication/Game/PieceFactory.cs
using ChessGameApplication.Game.Figures;
using ChessGameApplication.Windows;

namespace ChessGameApplication.Game
{
    public static class PieceFactory
    {
        public static Piece Create(PromotionPiece selectedPiece, PieceColor color, Position position)
        {
            return selectedPiece switch
            {
                PromotionPiece.Queen => new Queen(color, position),
                PromotionPiece.Rook => new Rook(color, position),
                PromotionPiece.Bishop => new Bishop(color, position),
                PromotionPiece.Knight => new Knight(color, position),
                _ => new Queen(color, position)
            };
        }

        public static Piece? Create(string typeName, PieceColor color, Position position, bool hasMoved)
        {
            return typeName switch
            {
                nameof(Pawn) => new Pawn(color, position, hasMoved),
                nameof(Rook) => new Rook(color, position, hasMoved),
                nameof(Knight) => new Knight(color, position, hasMoved),
                nameof(Bishop) => new Bishop(color, position, hasMoved),
                nameof(Queen) => new Queen(color, position, hasMoved),
                nameof(King) => new King(color, position, hasMoved),
                _ => null
            };
        }
    }
}
