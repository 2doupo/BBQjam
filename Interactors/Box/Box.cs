using Godot;
using System;

public partial class Box : Interactable
{

    public float duration;
    public float currentTime;
    private bool isUsed;

    public override void _Ready()
    {
        base._Ready();
        OnPlayerEnterZone += (i, p) => StartUse(p);
    }


    public void StartUse(player _player)
    {
        _player.GetInTheBox();
        QueueFree();
    }

    public void EndUse()
    {

    }

}
