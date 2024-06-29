using Godot;
using System;

public partial class GameOver : Control, IScene
{
    public void SetMainScene(MainScene scene, int currentLevel)
    {
        Label label = GetNode<Label>("Label");
        label.Text = "You Lost at Level " + currentLevel;

    }
}
