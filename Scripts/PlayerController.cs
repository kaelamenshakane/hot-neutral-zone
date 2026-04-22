using Godot;

public partial class PlayerController : CharacterBody2D
{
    [Export] public float MoveSpeed = 500f;

    private Node2D _aimPivot;
    private Hitbox _meleeHitbox;
    private GameManager _gameManager;

    public override void _Ready()
    {
        _aimPivot = GetNode<Node2D>("AimPivot");
        _meleeHitbox = GetNode<Hitbox>("AimPivot/MeleeHitbox");
        _gameManager = GetNodeOrNull<GameManager>("/root/GameManager");
    }

    public override void _PhysicsProcess(double delta)
    {
         if (Input.IsActionJustPressed("restart"))
        {
            if (_gameManager != null) _gameManager.RestartCurrentScene();
            else GetTree().ReloadCurrentScene();
            return;
        }
        if (Input.IsActionJustPressed("attack"))
        {
            _meleeHitbox.Activate();
        }
        Vector2 input = Vector2.Zero;
        if (Input.IsActionPressed("move_right")) input.X += 1;
        if (Input.IsActionPressed("move_left")) input.X -= 1;
        if (Input.IsActionPressed("move_down")) input.Y += 1;
        if (Input.IsActionPressed("move_up")) input.Y -= 1;

        if (input.Length() > 1) input = input.Normalized();
        Velocity = input * MoveSpeed;
        MoveAndSlide();

        Vector2 mousePos = GetGlobalMousePosition();
        Vector2 dir = mousePos - GlobalPosition;
        _aimPivot.Rotation = dir.Angle();
    }
}

