using System;
using UnityEngine;

public enum SoundType
{
    CHECK,
    SWOOSH,
    FAIL,
    MUSIC
}

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    [SerializeField] private AudioSource SFXSource;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip[] soundList;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        musicSource.clip = instance.soundList[(int)SoundType.MUSIC];
        musicSource.Play();
    }

    void Update()
    {
        if(!musicSource.isPlaying) 
        {
            musicSource.Play();
        }
    }

    public static void PlaySound(SoundType sound)
    {
        instance.SFXSource.PlayOneShot(instance.soundList[(int)sound]);
    }
}
