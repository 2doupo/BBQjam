using Godot;
using System;

public partial class InterfaceStart : Control, IScene
{

    private MainScene mainScene;

    public void SetMainScene(MainScene scene, int currentLevel)
    {
        mainScene = scene;
        TextureButton startButton = GetNode<TextureButton>("TextureButton");
        startButton.ButtonDown += () => scene.OpenNextLevel();
    }

}
