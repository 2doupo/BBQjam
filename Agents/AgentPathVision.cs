using Godot;
using Godot.NativeInterop;
using System;

public partial class AgentPathVision : AgentPath
{

    private Area2D visionZone;
    private Polygon2D visionVisual;

    private bool isPlayerInVision;
    private player _player;

    [Export]
    public Color neutralColor;
    [Export]
    public Color AlertedColor;

    private bool isPlayerCurrentlySeen;

    public bool IsPlayerCurrentlySeen
    {
        get => isPlayerCurrentlySeen;
        set
        {
            if (value) OverwriteTarget(_player.Transform.Origin, 80);
            else if (IsPlayerCurrentlySeen) OverwriteStop();
            isPlayerCurrentlySeen = value;
            visionVisual.Color = value ? AlertedColor : neutralColor;
        }
    }

    public override void _Ready()
    {
        base._Ready();
        visionZone = GetNode<Area2D>("Vision");
        visionVisual = GetNode<Polygon2D>("Vision/Polygon2D");
        IsPlayerCurrentlySeen = false;
        visionZone.BodyEntered += OnBodyEntered;
        visionZone.BodyExited += OnBodyExited;
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
            if (IsPlayerCurrentlySeen) IsPlayerCurrentlySeen = false;
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
            if (result["collider"].AsGodotObject() == player)
            {
                IsPlayerCurrentlySeen = true;
            }
            else if(IsPlayerCurrentlySeen)
            {
                IsPlayerCurrentlySeen = false;
            }
        }
    }

}
