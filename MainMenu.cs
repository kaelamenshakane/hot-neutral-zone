using Godot;

public partial class MainMenu : Control
{
    [Export] public PackedScene StartScene;

    private Button _newGameButton;
    private Button _continueButton;
    private Button _quitButton;
    private Button _gearButton;
    private Control _gearMenu;
    private Button _settingsButton;
    private Button _feedbackButton;

    public override void _Ready()
    {
        _newGameButton = GetNode<Button>("CenterContainer/VBoxContainer/NewGameButton");
        _continueButton = GetNode<Button>("CenterContainer/VBoxContainer/ContinueButton");
        _quitButton = GetNode<Button>("CenterContainer/VBoxContainer/QuitButton");

        _gearButton = GetNode<Button>("GearButton");
        _gearMenu = GetNode<Control>("GearMenu");
        _settingsButton = GetNode<Button>("GearMenu/VBoxContainer/SettingsButton");
        _feedbackButton = GetNode<Button>("GearMenu/VBoxContainer/FeedbackButton");

        _gearMenu.Visible = false;

        _newGameButton.Pressed += OnStartPressed;
        _continueButton.Pressed += OnStartPressed;
        _quitButton.Pressed += OnQuitPressed;

        _gearButton.Pressed += OnGearToggle;
        _settingsButton.Pressed += OnSettings;
        _feedbackButton.Pressed += OnFeedback;
    }

    private void OnStartPressed()
    {
        if (StartScene != null)
            GetTree().ChangeSceneToPacked(StartScene);
        else
            GD.PrintErr("StartScene не назначена в MainMenu.");
    }

    private void OnQuitPressed()
    {
        GetTree().Quit();
    }

    private void OnGearToggle()
    {
        _gearMenu.Visible = !_gearMenu.Visible;
    }

    private void OnSettings()
    {
        GD.Print("Открыть настройки (пока заглушка).");
    }

    private void OnFeedback()
    {
        GD.Print("Открыть обратную связь (пока заглушка).");
    }
}

