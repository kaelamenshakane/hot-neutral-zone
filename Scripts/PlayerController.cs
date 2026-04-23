using Godot;

public partial class PlayerController : CharacterBody2D
{
    [Signal] public delegate void WeaponChangedEventHandler(int weaponKind);

    [Export] public float MoveSpeed = 500f;

    private Node2D _aimPivot;
    private Hitbox _meleeHitbox;
    private Area2D _pickupArea;
    private Polygon2D _heldWeaponVisual;
    private PackedScene _weaponPickupScene;
    private PackedScene _projectileScene;
    private GameManager _gameManager;
    private WeaponKind _currentWeapon = WeaponKind.None;
    private WeaponPickup _nearbyWeapon;

    public WeaponKind CurrentWeapon => _currentWeapon;

    public override void _Ready()
    {
        _aimPivot = GetNode<Node2D>("AimPivot");
        _meleeHitbox = GetNode<Hitbox>("AimPivot/MeleeHitbox");
        _pickupArea = GetNode<Area2D>("PickupArea");
        _heldWeaponVisual = GetNodeOrNull<Polygon2D>("AimPivot/HeldWeaponVisual");
        _weaponPickupScene = ResourceLoader.Load<PackedScene>("res://Scenes/weapon_pickup.tscn");
        _projectileScene = ResourceLoader.Load<PackedScene>("res://Scenes/projectile.tscn");
        _gameManager = GetNodeOrNull<GameManager>("/root/GameManager");

        _pickupArea.AreaEntered += OnPickupAreaEntered;
        _pickupArea.AreaExited += OnPickupAreaExited;
        UpdateHeldWeaponVisual();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Input.IsActionJustPressed("restart"))
        {
            if (_gameManager != null) _gameManager.RestartCurrentScene();
            else GetTree().ReloadCurrentScene();
            return;
        }

        if (Input.IsActionJustPressed("pickup_throw"))
        {
            if (_currentWeapon == WeaponKind.None)
                TryPickupWeapon();
            else
                DropWeapon();
        }

        if (Input.IsActionJustPressed("attack"))
        {
            Attack();
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

    private void Attack()
    {
        if (_currentWeapon == WeaponKind.Pistol)
        {
            ShootPistol();
            return;
        }

        if (_currentWeapon == WeaponKind.Bat)
            _meleeHitbox.Activate(0.14f, 0.32f, 1.45f);
        else
            _meleeHitbox.Activate();
    }

    private void ShootPistol()
    {
        if (_projectileScene == null)
        {
            GD.PushError("Projectile scene is missing.");
            return;
        }

        Vector2 direction = Vector2.Right.Rotated(_aimPivot.GlobalRotation).Normalized();
        var projectile = _projectileScene.Instantiate<Projectile>();
        projectile.Direction = direction;
        projectile.OwnerRoot = this;
        GetTree().CurrentScene.AddChild(projectile);
        projectile.GlobalPosition = GlobalPosition + direction * 56.0f;
        projectile.GlobalRotation = direction.Angle();
    }

    private void TryPickupWeapon()
    {
        if (_nearbyWeapon == null || !IsInstanceValid(_nearbyWeapon))
            return;

        _currentWeapon = _nearbyWeapon.Kind;
        _nearbyWeapon.QueueFree();
        _nearbyWeapon = null;
        UpdateHeldWeaponVisual();
        EmitWeaponChanged();
    }

    private void DropWeapon()
    {
        if (_weaponPickupScene == null)
        {
            GD.PushError("Weapon pickup scene is missing.");
            return;
        }

        Vector2 direction = Vector2.Right.Rotated(_aimPivot.GlobalRotation).Normalized();
        var pickup = _weaponPickupScene.Instantiate<WeaponPickup>();
        pickup.Kind = _currentWeapon;
        GetTree().CurrentScene.AddChild(pickup);
        pickup.GlobalPosition = GlobalPosition + direction * 64.0f;
        pickup.GlobalRotation = direction.Angle();

        _currentWeapon = WeaponKind.None;
        UpdateHeldWeaponVisual();
        EmitWeaponChanged();
    }

    private void OnPickupAreaEntered(Area2D area)
    {
        if (area is WeaponPickup weaponPickup)
            _nearbyWeapon = weaponPickup;
    }

    private void OnPickupAreaExited(Area2D area)
    {
        if (area == _nearbyWeapon)
            _nearbyWeapon = null;
    }

    private void UpdateHeldWeaponVisual()
    {
        if (_heldWeaponVisual == null)
            return;

        _heldWeaponVisual.Visible = _currentWeapon != WeaponKind.None;

        if (_currentWeapon == WeaponKind.Pistol)
        {
            _heldWeaponVisual.Color = new Color(0.05f, 0.05f, 0.05f, 1.0f);
            _heldWeaponVisual.Polygon = new Vector2[]
            {
                new(14, -7), new(48, -7), new(48, 5), new(36, 5), new(36, 18), new(24, 18), new(24, 5), new(14, 5)
            };
        }
        else if (_currentWeapon == WeaponKind.Bat)
        {
            _heldWeaponVisual.Color = new Color(0.48f, 0.24f, 0.08f, 1.0f);
            _heldWeaponVisual.Polygon = new Vector2[]
            {
                new(10, -5), new(58, -5), new(66, 0), new(58, 5), new(10, 5)
            };
        }
    }

    private void EmitWeaponChanged()
    {
        EmitSignal(SignalName.WeaponChanged, (int)_currentWeapon);
    }
}
