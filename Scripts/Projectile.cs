using Godot;

public partial class Projectile : Area2D
{
    [Export] public float Speed = 1200.0f;
    [Export] public float Lifetime = 0.8f;

    public Vector2 Direction = Vector2.Right;
    public Node OwnerRoot;

    private double _age;

    public override void _Ready()
    {
        AreaEntered += OnAreaEntered;
        BodyEntered += OnBodyEntered;
    }

    public override void _PhysicsProcess(double delta)
    {
        GlobalPosition += Direction.Normalized() * Speed * (float)delta;
        _age += delta;

        if (_age >= Lifetime)
            QueueFree();
    }

    private void OnAreaEntered(Area2D area)
    {
        if (area is not Hurtbox hurtbox)
            return;

        if (OwnerRoot != null && hurtbox.GetParent() == OwnerRoot)
            return;

        hurtbox.ApplyHit();
        QueueFree();
    }

    private void OnBodyEntered(Node2D body)
    {
        if (OwnerRoot != null && body == OwnerRoot)
            return;

        if (body.GetNodeOrNull<Hurtbox>("Hurtbox") is Hurtbox hurtbox)
            hurtbox.ApplyHit();

        QueueFree();
    }
}
