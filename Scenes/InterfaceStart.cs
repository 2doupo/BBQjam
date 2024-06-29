using Godot;
using System;

public partial class InterfaceStart : Control, IScene
{

    private MainScene mainScene;

    public void SetMainScene(MainScene scene, int currentLevel)
    {
        mainScene = scene;
        Button startButton = GetNode<Button>("StartGame");
        startButton.ButtonDown += () => scene.OpenNextLeve();
    }

}
