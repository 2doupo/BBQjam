using Godot;
using System;

public partial class InterfaceInbetween : Control, IScene
{

    public void SetMainScene(MainScene scene, int currentLevel)
    {
        TextureButton startButton = GetNode<TextureButton>("TextureButton");
        startButton.Pressed += () => scene.OpenNextLevel();
    }
}
