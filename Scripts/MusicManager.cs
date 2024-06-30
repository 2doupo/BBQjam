using Godot;
using System;

public partial class MusicManager : Node
{

    public enum Music { MusicMain, MusicHidden }

    public const string musicMain = "MusicMain";
    public const string musicHidden = "MusicHidden";

    public static MusicManager instance;

    public override void _Ready()
    {
        base._Ready();
        instance = this;
    }

    public AudioStreamPlayer GetPlayer(Music value)
    {

        switch (value)
        {
            case Music.MusicMain:
                return GetNode<AudioStreamPlayer>(musicMain);
            case Music.MusicHidden:
                return GetNode<AudioStreamPlayer>(musicHidden);
        }
        return null;
    }

    public void PlayMusic(Music value)
    {
        GetPlayer(value).Play();
    }

    public void StopMusic(Music value)
    {
        GetPlayer(value).Stop();
    }

    public void MuteMusic(Music value)
    {
        GetPlayer(value).VolumeDb = -100;
    }

    public void UnMuteMusic(Music value)
    {
        GetPlayer(value).VolumeDb = 0;
    }

    public void StopAllMusics()
    {
        for (int i = 0; i < GetChildCount(); i++)
        {
            (GetChild(i) as AudioStreamPlayer).Stop();
        }
    }


}
