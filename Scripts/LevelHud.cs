using Godot;

public partial class LevelHud : CanvasLayer
{
    private Label _weaponStatus;
    private Control _deathPanel;
    private Control _pausePanel;
    private Button _resumeButton;
    private Button _mainMenuButton;
    private GameManager _gameManager;
    private PlayerController _player;
    private bool _playerDead;

    public override void _Ready()
    {
        ProcessMode = ProcessModeEnum.Always;

        _gameManager = GetNodeOrNull<GameManager>("/root/GameManager");
        _weaponStatus = GetNode<Label>("WeaponStatus");
        _deathPanel = GetNode<Control>("DeathPanel");
        _pausePanel = GetNode<Control>("PausePanel");
        _resumeButton = GetNode<Button>("PausePanel/MarginContainer/VBoxContainer/ResumeButton");
        _mainMenuButton = GetNode<Button>("PausePanel/MarginContainer/VBoxContainer/MainMenuButton");

        _deathPanel.Visible = false;
        _pausePanel.Visible = false;

        _resumeButton.Pressed += ResumeGame;
        _mainMenuButton.Pressed += ReturnToMainMenu;

        BindPlayer();
        UpdateWeaponStatus(_player?.CurrentWeapon ?? WeaponKind.None);
    }

    public override void _Process(double delta)
    {
        if (_playerDead)
        {
            if (Input.IsActionJustPressed("restart"))
                RestartLevel();

            return;
        }

        if (Input.IsActionJustPressed("pause_menu"))
            SetPauseVisible(!_pausePanel.Visible);
    }

    private void BindPlayer()
    {
        Node root = GetParent() ?? GetTree().CurrentScene;
        _player = root?.GetNodeOrNull<PlayerController>("Player");

        if (_player == null)
        {
            GD.PushWarning("LevelHud could not find Player node.");
            return;
        }

        _player.WeaponChanged += OnWeaponChanged;

        Hurtbox playerHurtbox = _player.GetNodeOrNull<Hurtbox>("Hurtbox");
        if (playerHurtbox != null)
            playerHurtbox.Killed += OnPlayerKilled;
        else
            GD.PushWarning("LevelHud could not find Player/Hurtbox.");
    }

    private void OnWeaponChanged(int weaponKind)
    {
        UpdateWeaponStatus((WeaponKind)weaponKind);
    }

    private void UpdateWeaponStatus(WeaponKind weaponKind)
    {
        if (_weaponStatus == null)
            return;

        string label = weaponKind switch
        {
            WeaponKind.Bat => "Bat",
            WeaponKind.Pistol => "Pistol",
            _ => "Hands"
        };

        _weaponStatus.Text = $"In hand: {label}";
    }

    private void OnPlayerKilled(Node killedNode)
    {
        if (_playerDead)
            return;

        _playerDead = true;
        _pausePanel.Visible = false;
        _deathPanel.Visible = true;
        GetTree().Paused = true;
    }

    private void RestartLevel()
    {
        GetTree().Paused = false;

        if (_gameManager != null)
            _gameManager.RestartCurrentScene();
        else
            GetTree().ReloadCurrentScene();
    }

    private void SetPauseVisible(bool visible)
    {
        _pausePanel.Visible = visible;
        GetTree().Paused = visible;
    }

    private void ResumeGame()
    {
        SetPauseVisible(false);
    }

    private void ReturnToMainMenu()
    {
        GetTree().Paused = false;

        if (_gameManager != null)
            _gameManager.ReturnToMainMenu();
        else
            GetTree().ChangeSceneToFile("res://Scenes/main_menu.tscn");
    }
}
