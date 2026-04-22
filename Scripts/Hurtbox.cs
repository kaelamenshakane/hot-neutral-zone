using Godot;

public partial class Hurtbox : Area2D
{
    [Export] public bool IsPlayer = false;
    private bool _dead;

    public void ApplyHit()
    {
        if (_dead) return;
        _dead = true;

        if (IsPlayer)
        {
            var gm = GetNodeOrNull<GameManager>("/root/GameManager");
            if (gm != null) gm.RestartCurrentScene();
            else GetTree().ReloadCurrentScene();
        }
        else
        {
            GetParent()?.QueueFree();
        }
    }
}
