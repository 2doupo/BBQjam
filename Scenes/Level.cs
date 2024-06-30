using Godot;
using System;
using System.Collections.Generic;

public partial class Level : Node2D, IScene
{

    private List<MailBox> AvailableBoxes;
    private Godot.Collections.Array<Node> CopCars;
    private Godot.Collections.Array<Node> Cameras;
    private int currentMailboxScore;
    private int maxMailboxScore;
    private MainScene mainScene;
    private int currentLevel;

    public float LevelTime = 62;
    private float currentTime;

    public InGameInterface gameInterface;


    private player _player;
    private Vector2 playerStartPosition;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Godot.Collections.Array<Node> mailboxes = GetTree().GetNodesInGroup("Mailbox");
        maxMailboxScore = mailboxes.Count;
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
        _player = GetTree().GetFirstNodeInGroup("Player") as player;
        playerStartPosition = _player.Position;
        (_player).OnPlayerCaught += PlayerCaught;

        GetNode<CameraManager>("Camera2D").SetPlayer(_player);
        gameInterface = GetTree().GetFirstNodeInGroup("Interface") as InGameInterface;
        gameInterface.SetProgress(0, LevelTime);
        gameInterface.SetMailCount(currentMailboxScore, maxMailboxScore);
        currentTime = 0;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (currentTime > LevelTime) return;
        currentTime += (float)delta;
        gameInterface.SetProgress(currentTime, LevelTime);
        if (currentTime > LevelTime) EndOfTime();
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
            AvailableBoxes.Remove(box);
            box.QueueFree();
            currentMailboxScore++;
            gameInterface.SetMailCount(currentMailboxScore, maxMailboxScore);
            MusicManager.instance.PlayMusic(MusicManager.Music.SoundPost);
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
        mainScene.FinishLevel(currentMailboxScore);
    }

    public void SetMainScene(MainScene scene, int currentLevel)
    {
        mainScene = scene;
        this.currentLevel = currentLevel;
    }

    private void PlayerCaught()
    {
        _player.ResetPosition(playerStartPosition);
    }

    private void EndOfTime()
    {
        mainScene.GameOver(currentMailboxScore);
    }
}
