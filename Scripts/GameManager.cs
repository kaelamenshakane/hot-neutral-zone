using Godot;

public partial class GameManager : Node
{
    public void RestartCurrentScene()
    {
        GetTree().ReloadCurrentScene();
    }
}
