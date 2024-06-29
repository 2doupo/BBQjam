using Godot;
using System;
using System.Collections.Generic;

public partial class VisionZone : Area2D
{

    [Signal]
    public delegate void OnPlayerSeenEventHandler(Vector2 position);
    [Signal]
    public delegate void OnPlayerLostEventHandler(Vector2 position);

    public delegate bool OverrideCheck(player p);
    public OverrideCheck check;

    private Polygon2D visionVisual;
    private bool isPlayerInVision;
    private player _player;

    public CharacterBody2D host;

    private bool isActive;
    public bool IsActive
    {
        get { return isActive; }
        set
        {
            if (!value)
            {
                isPlayerSpoted = false;
                isPlayerInVision = false;
                visionVisual.Color = neutralColor;
            }
            isActive = value;
            visionVisual.Visible = value;
            Monitoring = value;
        }
    }

    [Export]
    public Color neutralColor;
    [Export]
    public Color AlertedColor;

    private bool isPlayerSpoted;
    public bool IsPlayerSpoted
    {
        get { return isPlayerSpoted; }
        set
        {
            bool previousValue = isPlayerSpoted;
            isPlayerSpoted = value;
            visionVisual.Color = value ? AlertedColor : neutralColor;
            if (!previousValue && value) EmitSignal(SignalName.OnPlayerSeen, _player.Transform.Origin);
            if (previousValue && !value) EmitSignal(SignalName.OnPlayerLost, _player.Transform.Origin);
        }
    }


    public override void _Ready()
    {
        base._Ready();
        visionVisual = GetNode<Polygon2D>("Polygon2D");
        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExited;
    }

    public void Initialize(CharacterBody2D body, OnPlayerSeenEventHandler onPlayerSeen, OnPlayerLostEventHandler onPlayerLost, OverrideCheck checker, bool startState)
    {
        host = body;
        OnPlayerSeen += onPlayerSeen;
        OnPlayerLost += onPlayerLost;
        check = checker;
        IsActive = startState;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!IsActive) return;
        base._PhysicsProcess(delta);
        if (isPlayerInVision) PlayerSeen(_player);
    }

    private void OnBodyEntered(Node2D node)
    {
        if (node is player)
        {
            _player = node as player;
            isPlayerInVision = true;
        }
    }
    private void OnBodyExited(Node2D node)
    {
        if (node is player)
        {
            isPlayerInVision = false;
            if (IsPlayerSpoted) IsPlayerSpoted = false;
        }
    }
    private void PlayerSeen(Node2D player)
    {
        if (check(player as player))
        {
            if (!isPlayerSpoted) IsPlayerSpoted = true;
        }
        else
        {
            if (isPlayerSpoted) IsPlayerSpoted = false;
        }
    }

    public bool RaycastCheck(Node2D player)
    {
        PhysicsDirectSpaceState2D worldSpace = GetWorld2D().DirectSpaceState;
        PhysicsRayQueryParameters2D query = PhysicsRayQueryParameters2D.Create(host.Transform.Origin, player.Transform.Origin);
        query.Exclude = new Godot.Collections.Array<Rid> { GetRid() };
        query.HitFromInside = false;
        Godot.Collections.Dictionary result = worldSpace.IntersectRay(query);
        if (result.ContainsKey("collider"))
        {
            if (result["collider"].AsGodotObject() == player)
            {
                return true;
            }
        }
        return false;
    }

    public Vector2 GetPlayerPosition()
    {
        return _player.Transform.Origin;
    }

}
