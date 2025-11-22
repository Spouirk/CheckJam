using System;
using UnityEngine;

public class VolumeSliders : MonoBehaviour
{
    public void SetMusicVolume(float volume)
    {
        SoundManager.instance.SetMusicVolume(volume);
    }

    public void SetSFXVolume(float volume)
    {
        SoundManager.instance.SetSFXVolume(volume);
    }
}
