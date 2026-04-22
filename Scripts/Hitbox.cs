// Scripts/Hitbox.cs
using Godot;

public partial class Hitbox : Area2D
{
    [Export] public float ActiveTime = 0.1f;
    [Export] public float Cooldown = 0.3f;

    private bool _active;
    private bool _canAttack = true;
    private Node _ownerRoot;

    public override void _Ready()
    {
        _ownerRoot = GetParent()?.GetParent();
        Monitoring = false;
        AreaEntered += OnAreaEntered;
    }

    public async void Activate()
    {
        if (!_canAttack) return;

        _canAttack = false;
        _active = true;
        Monitoring = true;

        await ToSignal(GetTree(), "physics_frame");
        ApplyOverlaps();

        await ToSignal(GetTree().CreateTimer(ActiveTime), "timeout");
        Monitoring = false;
        _active = false;

        await ToSignal(GetTree().CreateTimer(Cooldown), "timeout");
        _canAttack = true;
    }

    private void OnAreaEntered(Area2D area)
    {
        if (!_active) return;
        Apply(area);
    }

    private void ApplyOverlaps()
    {
        foreach (Area2D area in GetOverlappingAreas())
        {
            Apply(area);
        }
    }

    private void Apply(Area2D area)
    {
        if (area is Hurtbox hb)
        {
            if (_ownerRoot != null && hb.GetParent() == _ownerRoot) return;
            hb.ApplyHit();
        }
    }
}

