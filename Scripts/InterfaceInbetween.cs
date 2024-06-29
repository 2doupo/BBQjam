using Godot;
using System;

public partial class InterfaceInbetween : Control, IScene
{



    public void SetMainScene(MainScene scene, int currentLevel)
    {
        Button startButton = GetNode<Button>("Button");
        startButton.Text = "Start Level: " + currentLevel;
        startButton.Pressed += () => scene.OpenNextLeve();
    }
}
