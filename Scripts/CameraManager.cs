using Godot;
using System;

public partial class CameraManager : Camera2D
{

    public player Player;

    public void SetPlayer(player p)
    {
        Player = p;
        Player.OnGetUnderBox += () => ZoomIn(1.2f);
        Player.OnGetOutOffBox += () => ZoomIn(1f);
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        if (Player != null) Position = Player.Position;
    }


    public void ZoomIn(float value)
    {

    }

}
