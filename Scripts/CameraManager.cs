using Godot;
using System;

public partial class CameraManager : Camera2D
{

    public player Player;

    private bool isShacking;
    private const float minShack = -3;
    public const float maxShack = 3;
    public const float shackSpeed = 0.7f;
    private double timer;
    private bool up;

    public void SetPlayer(player p)
    {
        Player = p;
        Player.OnGetUnderBox += () =>
        {
            ZoomIn(1.2f);
            StartShacking();
        };
        Player.OnGetOutOffBox += () =>
        {
            ZoomIn(1f);
            StopShacking();
        };
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        if (Player != null) Position = Player.Position;

        if (isShacking)
        {
            if (up)
            {
                timer += delta / shackSpeed;
                if (timer > 1) up = false;
            }
            else
            {
                timer -= delta / shackSpeed;
                if (timer < 0) up = true;
            }
            float res = Interpolation(minShack, maxShack, timer);
            RotationDegrees = res;
        }
    }


    public void ZoomIn(float value)
    {
        Zoom = Vector2.One * value;
    }

    public void StartShacking()
    {
        isShacking = true;
    }

    public void StopShacking()
    {
        isShacking = false;
        RotationDegrees = 0;
    }
    private float Interpolation(float a, float b, double t)
    {
        return a + (b - a) * (float)t;
    }
}
