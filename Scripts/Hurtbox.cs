using Godot;

public partial class Hurtbox : Area2D
{
    [Signal] public delegate void KilledEventHandler(Node killedNode);

    [Export] public bool IsPlayer = false;
    private bool _dead;

    public void ApplyHit()
    {
        if (_dead) return;
        _dead = true;

        if (IsPlayer)
        {
            EmitSignal(SignalName.Killed, GetParent());
        }
        else
        {
            Node parent = GetParent();
            EmitSignal(SignalName.Killed, parent);
            parent?.QueueFree();
        }
    }
}
