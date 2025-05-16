using ChessGameApplication.Game;
using ChessGameApplication.Game.Figures;
using ChessGameApplication.Game.PieceImageStrategies;
using ChessGameApplication.JsonModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChessGameApplication
{
    public class GameJsonOperator
    {
        private static readonly string GameSaveFilePath = "savedgame.json";

        public GameState? GameState { get; private set; }
        private readonly JsonSerializerOptions jsonSerializerOptions = new() { WriteIndented = true };

        private static GameJsonOperator? _instance;
        public static GameJsonOperator Instance => _instance ??= new GameJsonOperator();

        private GameJsonOperator() => Load();

        public void Load()
        {
            if (File.Exists(GameSaveFilePath))
            {
                var json = File.ReadAllText(GameSaveFilePath);
                GameState = JsonSerializer.Deserialize<GameState>(json);
            }
            else
            {
                GameState = new GameState();
            }
        }

        public void Save(PieceColor currentTurn, bool ckeckedState, IEnumerable<Piece> pieces)
        {
            GameState = new GameState
            {
                CurrentTurn = currentTurn,
                IsCheck = ckeckedState,
                Pieces = pieces
                .Select(p => new PieceModel
                {
                    Type = p.GetType().Name,
                    Color = p.Color,
                    Row = p.Position.Row,
                    Column = p.Position.Column,
                    HasMoved = p.HasMoved
                })
                .ToList()
            };

            var json = JsonSerializer.Serialize(GameState, jsonSerializerOptions);
            File.WriteAllText(GameSaveFilePath, json);
        }
    }
}
