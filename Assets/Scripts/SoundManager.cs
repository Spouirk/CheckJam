using System;
using UnityEngine;

public enum SoundType
{
    SWOOCH,
    SLAP,
    PUNCH,
    PINKY,
    KPOP,
    KON,
    KATON,
    JUL,
    HEARTBEAT,
    FAIL,
    MUSIC
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    [SerializeField] private AudioSource SFXSource;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip[] soundList;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
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

    public void SetMusicVolume(float volume)
    {
        Debug.Log("test");
        instance.musicSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        instance.SFXSource.volume = volume;
    }
}
