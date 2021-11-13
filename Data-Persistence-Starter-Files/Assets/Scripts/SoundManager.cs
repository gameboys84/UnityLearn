
using System;
using UnityEngine;

public enum SoundClip
{
    HitWall,
    HitBrick,
    HitPadel,
    
    GameWin = 100,
    GameLose,
}

public class SoundManager
{
    // private static SoundManager _instance;
    //
    // public static SoundManager Instance => _instance;
    
    // private void Awake()
    // {
    //     // DontDestroyOnLoad(this);
    //
    //     // if (_instance == null)
    //     // {
    //     //     _instance = GetComponent<SoundManager>();
    //     // }
    //     // else
    //     // {
    //     //     Destroy(this);
    //     //     return;
    //     // }
    // }

    public static void PlaySound(SoundClip clip)
    {
        string fileName; 
        switch (clip)
        {
            case SoundClip.HitWall:
                fileName = "wall";
                break;
            case SoundClip.HitBrick:
                fileName = "brick";
                break;
            case SoundClip.HitPadel:
                fileName = "paddle";
                break;
            case SoundClip.GameWin:
                fileName = "win";
                break;
            case SoundClip.GameLose:
                fileName = "lose";
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(clip), clip, null);
        }

        var audioClip = Resources.Load<AudioClip>($"Sounds/{fileName}");
        AudioSource.PlayClipAtPoint(audioClip, Vector3.zero);
    }

}
