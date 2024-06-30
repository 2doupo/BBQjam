using Godot;
using System;

public partial class GameOver : Control, IScene
{
    public void SetMainScene(MainScene scene, int currentLevel)
    {
        TextureProgressBar progressBar = GetNode<TextureProgressBar>("TextureProgressBar");
        progressBar.Value = scene.GetProgress() * 100;
    }
}
