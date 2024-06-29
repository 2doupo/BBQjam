using Godot;
using System;

public partial class player : CharacterBody2D
{
    public const float Speed = 60f;

    private NavigationAgent2D _navigationAgent;
    private bool isInit = false;

    public override void _Ready()
    {
        _navigationAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
        _navigationAgent.AvoidanceEnabled = true;
        _navigationAgent.VelocityComputed += VelocityComputed;
        Callable.From(ActorSetup).CallDeferred();
    }
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if (!isInit) return;


        Vector2 Direction = new Vector2(0, 0);
        if (Input.IsActionPressed("ui_up"))
        {
            Direction += new Vector2(0, -1);
        }
        if (Input.IsActionPressed("ui_down"))
        {
            Direction += new Vector2(0, 1);
        }
        if (Input.IsActionPressed("ui_left"))
        {
            Direction += new Vector2(-1, 0);
        }
        if (Input.IsActionPressed("ui_right"))
        {
            Direction += new Vector2(1, 0);
        }
        Direction = Direction.Normalized() * Speed;
        _navigationAgent.TargetPosition = Transform.Origin + Direction;
        _navigationAgent.Velocity = GlobalTransform.Origin.DirectionTo(_navigationAgent.GetNextPathPosition()) * Speed;

    }

    private void VelocityComputed(Vector2 safeVelocity)
    {
        this.Velocity = safeVelocity;
        MoveAndSlide();
    }

    private async void ActorSetup()
    {
        // Wait for the first physics frame so the NavigationServer can sync.
        await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
        isInit = true;
    }

}
