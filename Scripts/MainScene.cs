using Godot;
using System;

public partial class MainScene : Node
{

    public const string StartInterfacePath = "res://Scenes/InterfaceStart.tscn";
    public const string InBetweenInterface = "res://Scenes/InterfaceInbetween.tscn";
    public const string GameOverScene = "res://Scenes/GameOverScene.tscn";
    public const string Level1Path = "res://Scenes/level_2.tscn";
    public const string Level2Path = "res://Scenes/level_3.tscn";
    public const string Level3Path = "res://Scenes/level_4.tscn";
    public const string Level4Path = "res://Scenes/level_5.tscn";

    private int currentLevel = 1;
    private int currentScore = 0;

    private int lastLevel = 5;
    public override void _Ready()
    {
        base._Ready();
        LoadLevel(0);
    }


    public void LoadLevel(int index)
    {
        switch (index)
        {
            case 0: LoadLevel(StartInterfacePath); break;
            case 1: LoadLevel(InBetweenInterface); break;
            case 2: LoadLevel(GameOverScene); break;
            //case 3: LoadLevel(Level1Path); break;
            case 4: LoadLevel(Level2Path); break;
            case 5: LoadLevel(Level3Path); break;
            case 3: LoadLevel(Level4Path); break;
        }
    }

    public void LoadLevel(string path)
    {
        if (GetChildCount() > 0)
        {
            Node child = GetChild(0);
            child.QueueFree();
        }
        PackedScene scene = GD.Load<PackedScene>(path);
        Node sceneNode = scene.Instantiate();
        AddChild(sceneNode);

        if (sceneNode is IScene) (sceneNode as IScene).SetMainScene(this, currentLevel);
    }

    public void OpenNextLevel()
    {
        LoadLevel(currentLevel + 2);
    }

    public void FinishLevel(int score)
    {
        currentScore += score;
        currentLevel++;
        LoadLevel(1);
        if (currentLevel == lastLevel) GD.Print("last level");
    }

    public void GameOver(int score)
    {
        currentScore += score;
        LoadLevel(2);
    }

    public void Restart()
    {
        currentScore = 0;
        LoadLevel(0);
    }

}
