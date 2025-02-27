using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnStart : MonoBehaviour
{
    public SoundManager.Sound soundToPlay;

    private void Start()
    {
        SoundManager.Instance.PlaySoundAt(soundToPlay, transform.position);
    }
}
