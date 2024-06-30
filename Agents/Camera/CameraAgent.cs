using Godot;
using System;

public partial class CameraAgent : CharacterBody2D
{

    [Export]
    public VisionZone Vision;
    [Export]
    public float RotationMin;
    [Export]
    public float RotationMax;

    private bool isSeingPlayer;

    private float initialRotation;

    public float Speed = 2;
    private double timer;
    private bool up = true;

    [Signal]
    public delegate void OnCameraSeePlayerEventHandler(Vector2 position);

    public override void _Ready()
    {
        base._Ready();
        initialRotation = RotationDegrees;
        Vision.Initialize(
            this,
            (e) =>
            {
                isSeingPlayer = true;
                MusicManager.instance.PlayMusic(MusicManager.Music.SoundTalkie);
            },
            (e) =>
            {
                isSeingPlayer = false;
            },
            (p) => { return p.currentState == player.State.Free; },
            true
            );
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        if (isSeingPlayer)
        {
            EmitSignal(SignalName.OnCameraSeePlayer, Vision.GetPlayerPosition());
            return;
        }
        if (up)
        {
            timer += delta / Speed;
            if (timer > 1) up = false;
        }
        else
        {
            timer -= delta / Speed;
            if (timer < 0) up = true;
        }
        float res = Interpolation(RotationMin, RotationMax, timer);


        RotationDegrees = initialRotation + res;
    }

    private float Interpolation(float a, float b, double t)
    {
        return a + (b - a) * (float)t;
    }


}
