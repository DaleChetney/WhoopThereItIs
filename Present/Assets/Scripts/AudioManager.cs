using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoSingleton<AudioManager>
{
    public AudioSource bgm;
    public AudioSource knockBack;
    public AudioSource jump;
    public AudioSource slide;
    public AudioSource gruntAlert;
    public AudioSource gruntPass;
    public AudioSource gruntFail;
    public AudioSource mildAlarm;

    void Start()
    {
        DontDestroyOnLoad(this);

        bgm.Play();
    }
}
