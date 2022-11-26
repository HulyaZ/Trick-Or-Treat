using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioClip pickUp_Sound, dead_Sound;

    void Awake()
    {
        MakeInstance();
    }

    void Start()
    {
        
    }

    void MakeInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlayPickUpSound()
    {
        AudioSource.PlayClipAtPoint(pickUp_Sound, transform.position);
    }

    public void PlayDeadSound()
    {
        AudioSource.PlayClipAtPoint(dead_Sound, transform.position);
    }
}
