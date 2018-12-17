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

    // Start is called before the first frame update
    void Start()
    {
        bgm.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
