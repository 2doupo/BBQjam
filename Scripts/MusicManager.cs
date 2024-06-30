using Godot;
using System;

public partial class MusicManager : Node
{

    public enum Music { MusicMain, MusicHidden, SoundSpotted, MusicMenu, SoundDog, SoundPost , SoundDog2, SoundTalkie}

    public const string musicMain = "MusicMain";
    public const string musicHidden = "MusicHidden";
    public const string soundSpotted = "SoundSpotted";
    public const string musicMenu = "MusicMenu";
    public const string soundDog = "DogBark";
    public const string soundDog2 = "DogBark2";
    public const string soundPost = "SoundPost";
    public const string soundTalkie = "SoundTalkie";

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
            case Music.SoundSpotted:
                return GetNode<AudioStreamPlayer>(soundSpotted);
            case Music.MusicMenu:
                return GetNode<AudioStreamPlayer>(musicMenu);
            case Music.SoundDog:
                return GetNode<AudioStreamPlayer>(soundDog);
            case Music.SoundDog2:
                return GetNode<AudioStreamPlayer>(soundDog2);
            case Music.SoundPost:
                return GetNode<AudioStreamPlayer>(soundPost);
            case Music.SoundTalkie:
                return GetNode<AudioStreamPlayer>(soundTalkie);
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
