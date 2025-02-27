using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSliders : MonoBehaviour
{
    public Slider musicSlider;
    public Slider soundSlider;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("MusicVolume") || !PlayerPrefs.HasKey("SoundVolume"))
        {
            InitializeMusicAndSoundVolume();
            return;
        }


        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        soundSlider.value = PlayerPrefs.GetFloat("SoundVolume");

        SetMusicVolumeUI();
        SetSoundVolumeUI();
    }

    private void InitializeMusicAndSoundVolume()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("SoundVolume", soundSlider.value);
    }


    public void SetMusicVolumeUI()
    {
        MusicManager.Instance.SetVolume(musicSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
    }  
    public void SetSoundVolumeUI()
    {
        SoundManager.Instance.SetVolume(soundSlider.value);
        PlayerPrefs.SetFloat("SoundVolume", soundSlider.value);
    } 
    
    public void SetMusicVolume(float volume)
    {
        musicSlider.value = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }  
    public void SetSoundVolume(float volume)
    {
        soundSlider.value = volume;
        PlayerPrefs.SetFloat("SoundVolume", volume);
    }

}
