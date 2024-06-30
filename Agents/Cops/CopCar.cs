using Godot;
using System;

public partial class CopCar : AgentPath
{

    [Export]
    public VisionZone visionZone;
    private bool seesPlayer;

    public override void _Ready()
    {
        base._Ready();
        Area2D body = GetNode<Area2D>("Catch");
        body.BodyEntered += OnBodyEntered;
        visionZone.Initialize(
            this,
            (p) => seesPlayer = true,
            (p) =>
            {
                seesPlayer = false;
                OverwriteStop();
            },
            (p) =>
            {
                return p.currentState == player.State.Free && visionZone.RaycastCheck(p);
            },
            true);

    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if (seesPlayer) OverwriteTarget(visionZone.GetPlayerPosition(), 100);
    }

    public void OnBodyEntered(Node node)
    {
        if (node is player)
        {
            (node as player).PlayerCaught();
            ResetPosition();
        }
    }

}
