using UnityEngine;

public enum SoundType
{
    CHECK,
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

    private void start()
    {
        SFXSource = GetComponent<AudioSource>();
        musicSource = GetComponent<AudioSource>();
        musicSource.clip = instance.soundList[(int)SoundType.MUSIC];
        musicSource.Play();
    }

    public static void PlaySound(SoundType sound)
    {
        instance.SFXSource.PlayOneShot(instance.soundList[(int)sound]);
    }

    public static void SetMusicVolume(float volume)
    {
        instance.musicSource.volume = volume;
    }

    public static void SetSFXVolume(float volume)
    {
        instance.SFXSource.volume = volume;
    }
}
