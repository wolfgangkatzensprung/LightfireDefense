using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ToggleSound : MonoBehaviour
{
    public Text toggleSoundText;
    bool soundEnabled;

    void Start()
    {
        if (PlayerPrefs.GetInt("Sound") == 0)
        {
            DisableSound();
        }
        else
        {
            EnableSound();
        }
    }

    public void SetSoundToggle()
    {
        if (!soundEnabled)
        {
            EnableSound();
        }
        else if (soundEnabled)
        {
            DisableSound();
        }
    }

    void EnableSound()
    {
        toggleSoundText.text = "Sound enabled";
        PlayerPrefs.SetInt("Sound", 1);
        soundEnabled = true;
    }
    void DisableSound()
    {
        toggleSoundText.text = "Sound disabled";
        PlayerPrefs.SetInt("Sound", 0);
        soundEnabled = false;
    }
}
