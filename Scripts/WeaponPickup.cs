using Godot;

public partial class WeaponPickup : Area2D
{
    [Export] public WeaponKind Kind = WeaponKind.Bat;

    private Polygon2D _bodyVisual;
    private Label _label;

    public override void _Ready()
    {
        _bodyVisual = GetNodeOrNull<Polygon2D>("BodyVisual");
        _label = GetNodeOrNull<Label>("Label");

        ApplyVisuals();
    }

    private void ApplyVisuals()
    {
        if (_label != null)
            _label.Text = Kind.ToString();

        if (_bodyVisual == null)
            return;

        if (Kind == WeaponKind.Pistol)
        {
            _bodyVisual.Color = new Color(0.08f, 0.08f, 0.08f, 1.0f);
            _bodyVisual.Polygon = new Vector2[]
            {
                new(-24, -8), new(16, -8), new(16, 4), new(4, 4), new(4, 18), new(-8, 18), new(-8, 4), new(-24, 4)
            };
        }
        else
        {
            _bodyVisual.Color = new Color(0.45f, 0.22f, 0.08f, 1.0f);
            _bodyVisual.Polygon = new Vector2[]
            {
                new(-36, -5), new(30, -5), new(36, 0), new(30, 5), new(-36, 5)
            };
        }
    }
}
