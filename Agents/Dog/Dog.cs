using Godot;
using System;

public partial class Dog : AgentPath
{

    public enum DogState { Walking, Barking, Tracking, Following }
    public DogState currentState;

    [Export]
    public VisionZone CircularZone;
    [Export]
    public VisionZone TrackZone;

    public override void _Ready()
    {
        base._Ready();
        Area2D body = GetNode<Area2D>("Catch");
        body.BodyEntered += OnBodyEntered;
        CircularZone.IsActive = false;
        TrackZone.IsActive = false;
        currentState = DogState.Walking;
    }

    public void OnBodyEntered(Node node)
    {
        if (node is player)
        {
            (node as player).PlayerCaught();
        }
    }
}
