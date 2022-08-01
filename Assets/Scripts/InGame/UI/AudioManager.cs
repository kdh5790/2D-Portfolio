using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    AudioSource[] audioSource;
    public AudioClip mainBgm;
    public AudioClip buttonClip;


    void Awake()
    {
        instance = this;

        audioSource = GetComponents<AudioSource>();
        audioSource[0].clip = mainBgm;
        audioSource[0].Play();
        audioSource[0].loop = true;
    }


    public void PlayClip(AudioClip audio)
    {
        for (int i = 0; i < audioSource.Length; i++)
        {
            if (!audioSource[i].isPlaying)
            {
                audioSource[i].clip = null;
                audioSource[i].clip = audio;
                audioSource[i].Play();
                return;
            }
        }
    }

    public void PlayButtonClip()
    {
        for (int i = 0; i < audioSource.Length; i++)
        {
            if (!audioSource[i].isPlaying)
            {
                audioSource[i].clip = null;
                audioSource[i].clip = buttonClip;
                audioSource[i].Play();
                return;
            }
        }
    }
}
