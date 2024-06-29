using Godot;
using System;
using System.Security;

public partial class Dog : AgentPath
{

    public enum DogState { Walking, Barking, Tracking, Following }
    public DogState currentState;

    [Export]
    public VisionZone CircularZone;
    [Export]
    public VisionZone TrackZone;

    private double barkingTime = 1f;
    private double barkingTimer;

    public override void _Ready()
    {
        base._Ready();
        Area2D body = GetNode<Area2D>("Catch");
        body.BodyEntered += OnBodyEntered;
        CircularZone.IsActive = false;
        CircularZone.check = (p) =>
        {
            return true;
        };
        CircularZone.OnPlayerSeen += PlayerSeenCircular;
        TrackZone.IsActive = false;
        TrackZone.check = (p) =>
        {
            return p.currentState == player.State.Free;
        };
        currentState = DogState.Walking;
        _navigationAgent.TargetReached += OnTargetReached;
    }


    public override void _PhysicsProcess(double delta)
    {
        if (!isInit) return;
        switch (currentState)
        {
            case DogState.Walking:
                ProcessWalking(delta);
                break;
            case DogState.Barking:
                ProcessBarking(delta);
                break;
            case DogState.Tracking:
                ProcessTracking(delta);
                break;
            case DogState.Following:
                ProcessFollowing(delta);
                break;
        }
    }


    public void OnBodyEntered(Node node)
    {
        if (node is player)
        {
            (node as player).PlayerCaught();
        }
    }

    public void ChangeState(DogState newState)
    {
        ExitState(currentState);
        GD.Print("went from " + currentState + " to " + newState);
        currentState = newState;
        EnterState(currentState);
    }
    private void ExitState(DogState oldState)
    {
        switch (oldState)
        {
            case DogState.Walking:
                break;
            case DogState.Barking:
                CircularZone.IsActive = false;
                break;
            case DogState.Tracking:
                TrackZone.IsActive = false;
                OverwriteStop();
                break;
            case DogState.Following:
                break;
        }
    }
    private void EnterState(DogState newState)
    {
        switch (newState)
        {
            case DogState.Walking:
                NextStepPath();
                break;
            case DogState.Barking:
                CircularZone.IsActive = true;
                barkingTimer = 0;
                break;
            case DogState.Tracking:
                TrackZone.IsActive = true;
                break;
            case DogState.Following:
                break;
        }
    }
    private void ProcessWalking(double time)
    {
        _navigationAgent.Velocity = Transform.Origin.DirectionTo(_navigationAgent.GetNextPathPosition()) * currentSpeed;


    }
    private void ProcessBarking(double time)
    {
        barkingTimer += time;
        if (barkingTimer > barkingTime) ChangeState(DogState.Walking);
    }
    private void ProcessTracking(double time)
    {
        _navigationAgent.Velocity = Transform.Origin.DirectionTo(_navigationAgent.GetNextPathPosition()) * currentSpeed;
    }
    private void ProcessFollowing(double time)
    {


    }

    private void OnTargetReached()
    {
        switch (currentState)
        {
            case DogState.Walking:
                ChangeState(DogState.Barking);
                break;
            case DogState.Barking:
                break;
            case DogState.Tracking:
                ChangeState(DogState.Walking);
                break;
            case DogState.Following:
                break;
        }
    }

    private void PlayerSeenCircular(Vector2 position)
    {
        ChangeState(DogState.Tracking);
        OverwriteTarget(position, 75);
    }
}
