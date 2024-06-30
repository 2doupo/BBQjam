using Godot;
using System;

public partial class player : CharacterBody2D
{

    [Signal]
    public delegate void OnGetUnderBoxEventHandler();
    [Signal]
    public delegate void OnGetOutOffBoxEventHandler();

    public enum State { Free, InTheBox }
    private State _currentState = State.Free;
    public State currentState
    {
        get { return _currentState; }
        set
        {
            if (_currentState == State.InTheBox && value != State.InTheBox) EmitSignal(SignalName.OnGetOutOffBox);

            _currentState = value;
            boxSprite.Visible = currentState == State.InTheBox;
        }
    }

    public float CurrentSpeed
    {
        get
        {
            return currentState switch
            {
                State.Free => 80f,
                State.InTheBox => 30f,
                _ => 0
            };
        }
    }

    public const double BoxDuration = 4f;
    private double boxTimer;

    //private NavigationAgent2D _navigationAgent;
    private bool isInit = false;

    private Sprite2D boxSprite;

    [Signal]
    public delegate void OnPlayerCaughtEventHandler();

    public override void _Ready()
    {
        //_navigationAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
        //_navigationAgent.AvoidanceEnabled = true;
        //_navigationAgent.VelocityComputed += VelocityComputed;
        boxSprite = GetNode<Sprite2D>("Box");
        Callable.From(ActorSetup).CallDeferred();

    }
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if (!isInit) return;

        if (currentState == State.InTheBox)
        {
            boxTimer += delta;
            if (boxTimer > BoxDuration) currentState = State.Free;
        }

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
        Velocity = Direction.Normalized() * CurrentSpeed;
        //_navigationAgent.TargetPosition = Transform.Origin + Direction;
        //_navigationAgent.Velocity = GlobalTransform.Origin.DirectionTo(_navigationAgent.GetNextPathPosition()) * CurrentSpeed;

        MoveAndSlide();
    }

    private void VelocityComputed(Vector2 safeVelocity)
    {
        this.Velocity = safeVelocity;
    }

    private async void ActorSetup()
    {
        // Wait for the first physics frame so the NavigationServer can sync.
        await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
        isInit = true;
    }

    public void PlayerCaught()
    {
        EmitSignal(SignalName.OnPlayerCaught);
    }

    public void GetInTheBox()
    {
        boxTimer = 0;
        currentState = State.InTheBox;
    }

}
