// Scripts/Hitbox.cs
using Godot;

public partial class Hitbox : Area2D
{
    [Export] public float ActiveTime = 0.1f;
    [Export] public float Cooldown = 0.3f;

    private bool _active;
    private bool _canAttack = true;
    private Node _ownerRoot;
    private Vector2 _defaultScale;

    public override void _Ready()
    {
        _ownerRoot = GetParent()?.GetParent();
        _defaultScale = Scale;
        Monitoring = false;
        AreaEntered += OnAreaEntered;
    }

    public void Activate()
    {
        Activate(ActiveTime, Cooldown, 1.0f);
    }

    public async void Activate(float activeTime, float cooldown, float scaleMultiplier)
    {
        if (!_canAttack) return;

        _canAttack = false;
        _active = true;
        Scale = _defaultScale * scaleMultiplier;
        Monitoring = true;

        await ToSignal(GetTree(), "physics_frame");
        ApplyOverlaps();

        await ToSignal(GetTree().CreateTimer(activeTime), "timeout");
        Monitoring = false;
        _active = false;
        Scale = _defaultScale;

        await ToSignal(GetTree().CreateTimer(cooldown), "timeout");
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
