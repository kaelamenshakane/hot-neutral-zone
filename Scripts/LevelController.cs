using Godot;

public partial class LevelController : Node
{
    [Export] public int LevelNumber = 1;
    [Export] public NodePath EnemyPath = "Enemy";

    private GameManager _gameManager;
    private Hurtbox _enemyHurtbox;
    private bool _completed;

    public override void _Ready()
    {
        _gameManager = GetNodeOrNull<GameManager>("/root/GameManager");
        Node enemy = GetNodeOrNull<Node>(EnemyPath);
        _enemyHurtbox = enemy?.GetNodeOrNull<Hurtbox>("Hurtbox");

        if (_enemyHurtbox != null)
            _enemyHurtbox.Killed += OnEnemyKilled;
        else
            GD.PushWarning($"Level {LevelNumber} has no enemy hurtbox at path {EnemyPath}/Hurtbox.");
    }

    private void OnEnemyKilled(Node killedNode)
    {
        if (_completed)
            return;

        _completed = true;
        CallDeferred(MethodName.CompleteLevel);
    }

    private void CompleteLevel()
    {
        if (_gameManager != null)
            _gameManager.CompleteLevel(LevelNumber);
        else
            GD.PushError("GameManager autoload is missing; cannot complete level.");
    }
}
