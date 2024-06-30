using Godot;
using System;

public partial class InGameInterface : Control
{

    public TextureProgressBar progress;
    public Node label;

    public override void _Ready()
    {
        base._Ready();
        progress = GetNode<TextureProgressBar>("ProgressBar");
        label = GetNode<Node>("MailBoxCompteur");
    }

    public void SetProgress(float currentTime, float totalTime)
    {
        if (progress == null) progress = GetNode<TextureProgressBar>("ProgressBar");
        progress.Value = currentTime / totalTime * 100f;
    }

    public void SetMailCount(int current, int total)
    {
        if (label == null) label = GetNode<Node>("MailBoxCompteur");
        label.Call("set_value", current, total);
    }
}
