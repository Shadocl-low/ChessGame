using ChessGameApplication.Game.PieceImageStrategies;
using ChessGameApplication.JsonModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChessGameApplication
{
    public class SettingsJsonOperator
    {
        private static readonly string SettingsFilePath = "appsettings.json";

        public event Action<string>? WindowModeChanged;
        public event Action<IPieceImageStrategy>? PieceSkinChanged;

        public AppSettings? Settings { get; private set; }
        private readonly JsonSerializerOptions jsonSerializerOptions = new() { WriteIndented = true };

        private static SettingsJsonOperator? _instance;
        public static SettingsJsonOperator Instance => _instance ??= new SettingsJsonOperator();

        private SettingsJsonOperator() => Load();

        public void Load()
        {
            if (File.Exists(SettingsFilePath))
            {
                var json = File.ReadAllText(SettingsFilePath);
                Settings = JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
            }
            else
            {
                Settings = new AppSettings();
            }
        }

        public void Save()
        {
            var json = JsonSerializer.Serialize(Settings, jsonSerializerOptions);
            File.WriteAllText(SettingsFilePath, json);
        }
        public static void SetTheme(string theme)
        {
            Instance.Settings!.Theme = theme;
            Instance.Save();
        }
        public static void SetWindowMode(string windowMode)
        {
            Instance.Settings!.WindowMode = windowMode;
            Instance.Save();

            Instance.WindowModeChanged?.Invoke(windowMode);
        }
        public static void SetPieceSkin(string skin)
        {
            Instance.Settings!.PieceSkin = skin;
            Instance.Save();

            var strategy = GetImageStrategy();
            Instance.PieceSkinChanged?.Invoke(strategy);
        }
        public static IPieceImageStrategy GetImageStrategy()
        {
            IPieceImageStrategy strategy = Instance.Settings?.PieceSkin switch
            {
                "Cute" => new CutePieceImageStrategy(),
                _ => new ClassicPieceImageStrategy()
            };

            return strategy;
        }
    }
}
