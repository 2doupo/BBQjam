using Godot;
using System;

public partial class AgentPath : CharacterBody2D
{

    [Export]
    public Path2D path;

    public const float Speed = 100f;

    private NavigationAgent2D _navigationAgent;

    private int currentIndex = 0;
    private bool isInit = false;

    public override void _Ready()
    {
        _navigationAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
        _navigationAgent.AvoidanceEnabled = true;
        _navigationAgent.VelocityComputed += VelocityComputed;
        Transform = new Transform2D(0, path.Curve.GetPointPosition(currentIndex));
        currentIndex++;
        Callable.From(ActorSetup).CallDeferred();

    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if (!isInit) return;

        _navigationAgent.Velocity = Transform.Origin.DirectionTo(_navigationAgent.GetNextPathPosition()) * Speed;
    }

    private async void ActorSetup()
    {
        // Wait for the first physics frame so the NavigationServer can sync.
        await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
        isInit = true;
        _navigationAgent.TargetPosition = path.Curve.GetPointPosition(currentIndex);
    }
    private void VelocityComputed(Vector2 safeVelocity)
    {
        this.Velocity = safeVelocity;
        MoveAndSlide();
        if (_navigationAgent.IsTargetReached())
        {
            currentIndex = currentIndex < path.Curve.PointCount - 1 ? currentIndex + 1 : 0;
            _navigationAgent.TargetPosition = path.Curve.GetPointPosition(currentIndex);
        }
    }

}
