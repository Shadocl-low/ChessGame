using ChessGameApplication.Game;
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
    public class StatsJsonOperator
    {
        private static readonly string StatsFilePath = "gamestats.json";

        public GameStats Stats { get; private set; }
        private readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };

        private static StatsJsonOperator? _instance;
        public static StatsJsonOperator Instance => _instance ??= new StatsJsonOperator();

        private StatsJsonOperator() => Load();

        public void Load()
        {
            if (File.Exists(StatsFilePath))
            {
                var json = File.ReadAllText(StatsFilePath);
                Stats = JsonSerializer.Deserialize<GameStats>(json) ?? new GameStats();
            }
            else
            {
                Stats = new GameStats();
            }
        }

        public void Save()
        {
            var json = JsonSerializer.Serialize(Stats, _jsonOptions);
            File.WriteAllText(StatsFilePath, json);
        }

        public void RecordGameResult(GameEndResult result)
        {
            if (result.Reason == EndReason.Checkmate && result.Winner.HasValue)
            {
                if (result.Winner == PieceColor.White)
                    Stats.WhiteWins++;
                else
                    Stats.BlackWins++;
            }
            else if (result.Reason == EndReason.Stalemate)
            {
                Stats.Draws++;
            }

            Save();
        }

        public void ResetStats()
        {
            Stats = new GameStats();
            Save();
        }
    }
}
