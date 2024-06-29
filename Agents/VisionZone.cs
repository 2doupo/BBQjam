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

    private bool isActive;
    public bool IsActive
    {
        get { return isActive; }
        set
        {
            isActive = value;
            visionVisual.Visible = value;
            Monitoring = value;
            Monitorable = value;
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
            if (isPlayerSpoted && !value) EmitSignal(SignalName.OnPlayerLost, _player.Transform.Origin);
            if (!isPlayerSpoted && value) EmitSignal(SignalName.OnPlayerSeen, _player.Transform.Origin);
            isPlayerSpoted = value;
            visionVisual.Color = value ? AlertedColor : neutralColor;
        }
    }


    public override void _Ready()
    {
        base._Ready();
        visionVisual = GetNode<Polygon2D>("Polygon2D");

        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExited;
    }

    public override void _PhysicsProcess(double delta)
    {
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
        PhysicsDirectSpaceState2D worldSpace = GetWorld2D().DirectSpaceState;
        PhysicsRayQueryParameters2D query = PhysicsRayQueryParameters2D.Create(Transform.Origin, player.Transform.Origin);
        query.Exclude = new Godot.Collections.Array<Rid> { GetRid() };
        Godot.Collections.Dictionary result = worldSpace.IntersectRay(query);
        if (result.ContainsKey("collider"))
        {
            if (result["collider"].AsGodotObject() == player && check(_player))
            {
                if (!IsPlayerSpoted) IsPlayerSpoted = true;
            }
            else if (IsPlayerSpoted)
            {
                IsPlayerSpoted = false;
            }
        }
    }

    public Vector2 GetPlayerPosition()
    {
        return _player.Transform.Origin;
    }

}
