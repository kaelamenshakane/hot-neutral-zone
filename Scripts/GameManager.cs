using Godot;
using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

public partial class GameManager : Node
{
    private const string SavePath = "user://save.json";
    private const int DefaultUnlockedLevel = 1;

    private readonly string[] _levelPaths =
    {
        "res://Scenes/level_1.tscn",
        "res://Scenes/level_2.tscn",
        "res://Scenes/level_3.tscn"
    };

    private int _currentLevelNumber;
    private int _highestUnlockedLevel = DefaultUnlockedLevel;

    public override void _Ready()
    {
        LoadSave();
    }

    public int TotalLevels => _levelPaths.Length;

    public int HighestUnlockedLevel => _highestUnlockedLevel;

    public void StartNewGame()
    {
        _highestUnlockedLevel = DefaultUnlockedLevel;
        _currentLevelNumber = DefaultUnlockedLevel;
        SaveProgress();
        LoadLevel(DefaultUnlockedLevel);
    }

    public bool IsLevelUnlocked(int levelNumber)
    {
        return levelNumber >= 1 && levelNumber <= _highestUnlockedLevel && levelNumber <= TotalLevels;
    }

    public void LoadLevel(int levelNumber)
    {
        LoadSave();

        if (!IsLevelUnlocked(levelNumber))
        {
            GD.PushWarning($"Level {levelNumber} is locked.");
            return;
        }

        _currentLevelNumber = levelNumber;
        GetTree().ChangeSceneToFile(_levelPaths[levelNumber - 1]);
    }

    public void CompleteLevel(int levelNumber)
    {
        if (levelNumber < 1 || levelNumber > TotalLevels)
        {
            GD.PushWarning($"Cannot complete invalid level {levelNumber}.");
            return;
        }

        _currentLevelNumber = levelNumber;

        if (levelNumber < TotalLevels)
            _highestUnlockedLevel = Mathf.Max(_highestUnlockedLevel, levelNumber + 1);
        else
            _highestUnlockedLevel = TotalLevels;

        SaveProgress();

        if (levelNumber < TotalLevels)
            LoadLevel(levelNumber + 1);
        else
            GetTree().ChangeSceneToFile("res://Scenes/main_menu.tscn");
    }

    public void CompleteCurrentLevel()
    {
        if (_currentLevelNumber <= 0)
        {
            GD.PushWarning("No current level is active.");
            return;
        }

        CompleteLevel(_currentLevelNumber);
    }

    public void RestartCurrentScene()
    {
        GetTree().ReloadCurrentScene();
    }

    public void ResetSave()
    {
        _currentLevelNumber = 0;
        _highestUnlockedLevel = DefaultUnlockedLevel;

        string globalPath = ProjectSettings.GlobalizePath(SavePath);
        if (File.Exists(globalPath))
            File.Delete(globalPath);
    }

    private void LoadSave()
    {
        string globalPath = ProjectSettings.GlobalizePath(SavePath);
        if (!File.Exists(globalPath))
        {
            _highestUnlockedLevel = DefaultUnlockedLevel;
            return;
        }

        try
        {
            string json = File.ReadAllText(globalPath);
            SaveData saveData = JsonSerializer.Deserialize<SaveData>(json) ?? new SaveData();
            _highestUnlockedLevel = Mathf.Clamp(saveData.HighestUnlockedLevel, DefaultUnlockedLevel, TotalLevels);
        }
        catch (Exception exception)
        {
            GD.PushWarning($"Failed to read save file. Falling back to level 1. {exception.Message}");
            _highestUnlockedLevel = DefaultUnlockedLevel;
        }
    }

    private void SaveProgress()
    {
        string globalPath = ProjectSettings.GlobalizePath(SavePath);
        string directory = Path.GetDirectoryName(globalPath);
        if (!string.IsNullOrEmpty(directory))
            Directory.CreateDirectory(directory);

        var saveData = new SaveData
        {
            HighestUnlockedLevel = Mathf.Clamp(_highestUnlockedLevel, DefaultUnlockedLevel, TotalLevels)
        };

        var options = new JsonSerializerOptions { WriteIndented = true };
        File.WriteAllText(globalPath, JsonSerializer.Serialize(saveData, options));
    }

    private sealed class SaveData
    {
        [JsonPropertyName("highest_unlocked_level")]
        public int HighestUnlockedLevel { get; set; } = DefaultUnlockedLevel;
    }
}
