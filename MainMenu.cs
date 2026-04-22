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
    private Label _titleLabel;

    private float _titleTime;
    private Vector2 _titleBasePosition;

    public override void _Ready()
    {
        _newGameButton = GetNode<Button>("NewGame");
        _continueButton = GetNode<Button>("ContinueGame");
        _quitButton = GetNode<Button>("Exit");
        _titleLabel = GetNodeOrNull<Label>("Label");

        _gearButton = GetNodeOrNull<Button>("GearButton");
        _gearMenu = GetNodeOrNull<Control>("GearMenu");
        _settingsButton = GetNodeOrNull<Button>("GearMenu/VBoxContainer/SettingsButton");
        _feedbackButton = GetNodeOrNull<Button>("GearMenu/VBoxContainer/FeedbackButton");

        if (_titleLabel != null)
        {
            _titleBasePosition = _titleLabel.Position;
            _titleLabel.PivotOffset = _titleLabel.Size * 0.5f;
        }

        if (_gearMenu != null)
            _gearMenu.Visible = false;

        _newGameButton.Pressed += OnStartPressed;
        _continueButton.Pressed += OnStartPressed;
        _quitButton.Pressed += OnQuitPressed;

        if (_gearButton != null)
            _gearButton.Pressed += OnGearToggle;
        if (_settingsButton != null)
            _settingsButton.Pressed += OnSettings;
        if (_feedbackButton != null)
            _feedbackButton.Pressed += OnFeedback;
    }

    public override void _Process(double delta)
    {
        if (_titleLabel == null)
            return;

        _titleTime += (float)delta;

        float sway = Mathf.Sin(_titleTime * 1.6f);
        float textHueShift = Mathf.Sin(_titleTime * 1.1f) * 0.05f;
        float textHue = 0.96f + textHueShift;
        if (textHue > 1f) textHue -= 1f;
        if (textHue < 0f) textHue += 1f;

        // Outline hue moves in the red-violet band.
        float outlineLerp = (Mathf.Sin(_titleTime * 1.4f) + 1f) * 0.5f;
        Color outlineColor = Color.FromHsv(Mathf.Lerp(0.0f, 0.82f, outlineLerp), 0.85f, 0.85f, 1f);
        Color textColor = Color.FromHsv(textHue, 0.58f, 0.38f, 1f);

        _titleLabel.Position = _titleBasePosition + new Vector2(sway * 8f, 0f);
        _titleLabel.RotationDegrees = sway * 2.4f;
        _titleLabel.AddThemeColorOverride("font_color", textColor);
        _titleLabel.AddThemeColorOverride("font_outline_color", outlineColor);
    }

    private void OnStartPressed()
    {
        if (StartScene != null)
            GetTree().ChangeSceneToPacked(StartScene);
        else
            GD.PrintErr("StartScene is not assigned in MainMenu.");
    }

    private void OnQuitPressed()
    {
        GetTree().Quit();
    }

    private void OnGearToggle()
    {
        if (_gearMenu != null)
            _gearMenu.Visible = !_gearMenu.Visible;
    }

    private void OnSettings()
    {
        GD.Print("Settings clicked (placeholder).");
    }

    private void OnFeedback()
    {
        GD.Print("Feedback clicked (placeholder).");
    }
}
