using Godot;
using System;

public partial class CopCar : AgentPathVision
{

    public override void _Ready()
    {
        base._Ready();
        Area2D body = GetNode<Area2D>("Catch");
        body.BodyEntered += OnBodyEntered;
        GD.Print("register catch");
    }

    public void OnBodyEntered(Node node)
    {
        GD.Print("hit player");

        if (node is player)
        {
            (node as player).PlayerCaught();
        }
    }

}
