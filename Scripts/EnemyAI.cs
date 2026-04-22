using Godot;
using System.Collections.Generic;

public partial class EnemyAI : CharacterBody2D
{
    [Export] public float MoveSpeed = 220f;
    [Export] public float LoseSightDelay = 1.2f;

    private Area2D _visionArea;
    private Area2D _attackArea;
    private Node2D _patrolPointsRoot;
    private Node2D _player;

    private Vector2[] _patrolPoints = new Vector2[0];
    private int _patrolIndex;
    private bool _playerInVision;
    private double _lostTimer;

    public override void _Ready()
    {
        _visionArea = GetNode<Area2D>("VisionArea");
        _attackArea = GetNode<Area2D>("AttackArea");
        _patrolPointsRoot = GetNodeOrNull<Node2D>("PatrolPoints");

        _visionArea.BodyEntered += OnVisionBodyEntered;
        _visionArea.BodyExited += OnVisionBodyExited;
        _attackArea.AreaEntered += OnAttackAreaEntered;

        CachePatrolPoints();
        FindPlayer();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_player == null) FindPlayer();

        bool canSee = _playerInVision && _player != null && HasLineOfSight(_player.GlobalPosition);

        if (canSee)
        {
            _lostTimer = 0;
            MoveTowards(_player.GlobalPosition);
        }
        else
        {
            _lostTimer += delta;
            if (_lostTimer >= LoseSightDelay)
            {
                Patrol();
            }
            else
            {
                Velocity = Vector2.Zero;
                MoveAndSlide();
            }
        }
    }

    private void CachePatrolPoints()
    {
        if (_patrolPointsRoot == null)
        {
            _patrolPoints = new Vector2[0];
            return;
        }

        var list = new List<Vector2>();
        foreach (Node child in _patrolPointsRoot.GetChildren())
        {
            if (child is Node2D p) list.Add(p.GlobalPosition);
        }
        _patrolPoints = list.ToArray();
    }

    private void FindPlayer()
    {
        _player = GetTree().Root.FindChild("Player", true, false) as Node2D;
    }

    private void Patrol()
    {
        if (_patrolPoints.Length == 0)
        {
            Velocity = Vector2.Zero;
            MoveAndSlide();
            return;
        }

        Vector2 target = _patrolPoints[_patrolIndex];
        if (GlobalPosition.DistanceTo(target) < 8f)
        {
            _patrolIndex = (_patrolIndex + 1) % _patrolPoints.Length;
            target = _patrolPoints[_patrolIndex];
        }

        MoveTowards(target);
    }

    private void MoveTowards(Vector2 target)
    {
        Vector2 dir = target - GlobalPosition;
        if (dir.Length() > 1) dir = dir.Normalized();
        Velocity = dir * MoveSpeed;
        MoveAndSlide();
    }

    private bool HasLineOfSight(Vector2 target)
    {
        var space = GetWorld2D().DirectSpaceState;
        var query = PhysicsRayQueryParameters2D.Create(GlobalPosition, target);
        query.Exclude = new Godot.Collections.Array<Rid> { GetRid() };

        var result = space.IntersectRay(query);
        if (result.Count == 0) return false;

        var collider = result["collider"].As<Node>();
        if (collider == null || _player == null) return false;

        return collider == _player || _player.IsAncestorOf(collider);
    }

    private void OnVisionBodyEntered(Node2D body)
    {
        if (body.Name == "Player") _playerInVision = true;
    }

    private void OnVisionBodyExited(Node2D body)
    {
        if (body.Name == "Player") _playerInVision = false;
    }

    private void OnAttackAreaEntered(Area2D area)
    {
        if (area is Hurtbox hb) hb.ApplyHit();
    }
}
