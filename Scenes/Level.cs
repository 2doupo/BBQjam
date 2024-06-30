using Godot;
using System;
using System.Collections.Generic;

public partial class Level : Node2D, IScene
{

    private List<MailBox> AvailableBoxes;
    private Godot.Collections.Array<Node> CopCars;
    private Godot.Collections.Array<Node> Cameras;
    private int currentMailboxScore;
    private MainScene mainScene;
    private int currentLevel;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Godot.Collections.Array<Node> mailboxes = GetTree().GetNodesInGroup("Mailbox");
        foreach (Node item in mailboxes)
        {
            RegisterMailbox(item as MailBox);
        }
        CopCars = GetTree().GetNodesInGroup("Cop");
        Cameras = GetTree().GetNodesInGroup("Camera");
        foreach (Node item in Cameras)
        {
            (item as CameraAgent).OnCameraSeePlayer += CallAllAgentsTo;
        }
        (GetTree().GetFirstNodeInGroup("Player") as player).OnPlayerCaught += GameOver;
    }


    public void RegisterMailbox(MailBox mailBox)
    {
        if (AvailableBoxes == null) AvailableBoxes = new List<MailBox>();

        AvailableBoxes.Add(mailBox);
        mailBox.OnPlayerEnterZone += (node, player) => InteractedWithMailbox(mailBox);
    }

    public void InteractedWithMailbox(MailBox box)
    {
        if (AvailableBoxes.Contains(box))
        {
            Sprite2D sprite = box.GetNode<Sprite2D>("Sprite2D");
            sprite.Modulate = new Color(0, 1, 0, 1);
            AvailableBoxes.Remove(box);
            if (AvailableBoxes.Count == 0) LevelEnded();
        }
    }

    public void CallAllAgentsTo(Vector2 position)
    {
        foreach (Node item in CopCars)
        {
            (item as CopCar).OverwriteTarget(position, 80);
        }
    }

    public void LevelEnded()
    {
        mainScene.FinishLevel();
    }

    public void SetMainScene(MainScene scene, int currentLevel)
    {
        mainScene = scene;
        this.currentLevel = currentLevel;
    }

    private void GameOver()
    {
        mainScene.GameOver();
    }
}
