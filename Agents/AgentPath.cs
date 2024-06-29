using Godot;
using System;

public partial class AgentPath : CharacterBody2D
{

    [Export]
    public Path2D path;

    [Export]
    public float Speed = 60f;
    [Export]
    public bool AutoPath = true;

    protected float currentSpeed;

    protected NavigationAgent2D _navigationAgent;

    private int currentIndex = 0;
    protected bool isInit = false;
    private bool IsOverwriten = false;

    public override void _Ready()
    {
        _navigationAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
        _navigationAgent.AvoidanceEnabled = true;
        _navigationAgent.VelocityComputed += VelocityComputed;
        Transform = new Transform2D(0, path.Curve.GetPointPosition(currentIndex));
        currentIndex++;
        currentSpeed = Speed;
        Callable.From(ActorSetup).CallDeferred();
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if (!isInit) return;

        _navigationAgent.Velocity = Transform.Origin.DirectionTo(_navigationAgent.GetNextPathPosition()) * currentSpeed;
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
        Rotation = safeVelocity.Angle() + Mathf.Pi;
        MoveAndSlide();
        if (_navigationAgent.IsTargetReached() && AutoPath)
        {
            NextStepPath();
        }
    }


    public void NextStepPath()
    {
        currentIndex = currentIndex < path.Curve.PointCount - 1 ? currentIndex + 1 : 0;
        _navigationAgent.TargetPosition = path.Curve.GetPointPosition(currentIndex);
    }

    public void OverwriteTarget(Vector2 position, float speed)
    {
        IsOverwriten = true;
        _navigationAgent.TargetPosition = position;
        currentSpeed = speed;
    }

    public void OverwriteStop()
    {
        _navigationAgent.TargetPosition = path.Curve.GetPointPosition(currentIndex);
        currentSpeed = Speed;
        IsOverwriten = false;
    }

}
