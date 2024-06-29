using Godot;
using System;
using System.Collections.Generic;

public partial class Level : Node2D, IScene
{

    private List<MailBox> AvailableBoxes;
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

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }


    public void RegisterMailbox(MailBox mailBox)
    {
        if (AvailableBoxes == null) AvailableBoxes = new List<MailBox>();

        AvailableBoxes.Add(mailBox);
        mailBox.OnPlayerEnterZone += (node) => InteractedWithMailbox(mailBox);
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


    public void LevelEnded()
    {
        mainScene.FinishLevel();
    }

    public void SetMainScene(MainScene scene, int currentLevel)
    {
        mainScene = scene;
        this.currentLevel = currentLevel;
    }
}
