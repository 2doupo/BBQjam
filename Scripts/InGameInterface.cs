using Godot;
using System;

public partial class InGameInterface : Control
{

    public ProgressBar progress;
    public Label label;

    public override void _Ready()
    {
        base._Ready();
        progress = GetNode<ProgressBar>("ProgressBar");
        label = GetNode<Label>("Label");
    }

    public void SetProgress(float currentTime, float totalTime)
    {
        if(progress == null) progress = GetNode<ProgressBar>("ProgressBar");
        progress.Value = currentTime / totalTime * 100f;
    }

    public void SetMailCount(int current, int total)
    {
        if(label == null) label = GetNode<Label>("Label");
        label.Text = current + " / " + total;
    }
}
