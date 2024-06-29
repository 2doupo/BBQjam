using Godot;
using System;

public partial class Interactable : Area2D
{

    [Signal]
    public delegate void OnPlayerEnterZoneEventHandler(Interactable interactable, player Player);

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }


    private void OnBodyEntered(Node2D otherNode)
    {
        if (otherNode is player) EmitSignal(SignalName.OnPlayerEnterZone, this, otherNode as player);
    }



}
