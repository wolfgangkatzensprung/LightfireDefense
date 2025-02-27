using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : Singleton<SoundManager>
{
    List<AudioSource> audioSources = new List<AudioSource>();
    public AudioSource unspacialSource;
    public AudioSource playerAttackSoundscapeSource;
    public AudioSource enemySoundscapeSource;

    public enum Sound
    {
        PlayerJump,
        PlayerDamaged,
        EarthProjectile,
        WaterProjectile,
        TurretProjectile,
        PlayerDash,
        ProjectileHit,
        SpellField,
        EnemyDamaged,
        EnemyDeath,
        Interact,
        Error
    }

    [Header("Player related Sound FX")]
    public AudioClip playerJump;
    public AudioClip playerDamaged;
    public AudioClip earthProjectile;
    public AudioClip waterProjectile;
    public AudioClip fireAttack;
    public AudioClip airAttack;
    public AudioClip playerDash;
    public AudioClip projectileHit;
    public AudioClip spellField;
    public AudioClip turretProjectile;

    [Header("Enemy related Sound FX")]
    public AudioClip enemyDamaged;
    public AudioClip enemyDeath;
    public AudioClip enemySoundscape;

    [Header("Miscellanious Sound FX")]
    public AudioClip interact;
    public AudioClip error;

    Transform firePoint;
    Transform playerTrans;

    private void Start()
    {
        firePoint = GlobalInfo.Instance.firePoint;
        playerTrans = GlobalInfo.Instance.playerTrans;

        foreach (Transform child in transform)
        {
            if (child.TryGetComponent(out AudioSource audio))
            {
                audioSources.Add(audio);
            }
        }
    }

    public void PlaySoundAt(Sound sound, Vector3 point)
    {
        AudioClip clip = null;
        clip = SoundSwitch(sound, clip);

        if (clip != null)
        {
            PlayClipAtPoint(clip, point);
        }
    }
    public void PlayNonspacialSound(Sound sound)
    {
        AudioClip clip = null;
        clip = SoundSwitch(sound, clip);

        if (clip != null)
        {
            unspacialSource.clip = clip;
            unspacialSource.Play();
        }
    }

    private AudioClip SoundSwitch(Sound sound, AudioClip clip)
    {
        switch (sound)
        {
            case Sound.ProjectileHit:
                clip = projectileHit;
                break;
            case Sound.SpellField:
                clip = spellField;
                break;
            case Sound.PlayerJump:
                clip = playerJump;
                break;
            case Sound.PlayerDamaged:
                clip = playerDamaged;
                break;
            case Sound.EarthProjectile:
                clip = earthProjectile;
                break;    
            case Sound.WaterProjectile:
                clip = waterProjectile;
                break;  
            case Sound.TurretProjectile:
                clip = turretProjectile;
                break;
            case Sound.PlayerDash:
                clip = playerDash;
                break;
            case Sound.EnemyDamaged:
                clip = enemyDamaged;
                break;
            case Sound.EnemyDeath:
                clip = enemyDeath;
                break;
            case Sound.Interact:
                clip = interact;
                break;
            case Sound.Error:
                clip = error;
                break;
        }

        return clip;
    }

    private void PlayClipAtPoint(AudioClip clip, Vector3 point)
    {
        StartCoroutine(PlayClipAtPointRoutine(clip, point));
    }

    IEnumerator PlayClipAtPointRoutine(AudioClip clip, Vector3 point)
    {
        AudioSource audio = GetAvailableSource();
        audio.clip = null;
        audio.transform.position = point;
        yield return new WaitForEndOfFrame();
        audio.clip = clip;

        audio.pitch = 1 + Random.Range(-.1f, .1f);

        audio.Play();
    }

    internal void SetVolume(float soundVolume)
    {
        foreach (AudioSource audio in audioSources)
        {
            audio.volume = soundVolume;
        }
    }

    internal AudioSource GetAvailableSource()
    {
        AudioSource longestPlayingSource = new AudioSource();

        foreach (AudioSource audio in audioSources)
        {
            float longestTime = 0f;

            if (!audio.isPlaying)
                return audio;
            else if (audio.time >= longestTime)      // Wenn alle Audios besetzt sind, wird das mit der laengsten Playtime returnt
            {
                longestTime = audio.time;
                longestPlayingSource = audio;
            }
        }

        return longestPlayingSource;
    }

    internal void PlayAttackSoundscape(bool isAir)
    {
        if (isAir)
        {
            if (!playerAttackSoundscapeSource.isPlaying)
            {
                playerAttackSoundscapeSource.clip = airAttack;
                playerAttackSoundscapeSource.Play();
            }
        }
        else if (!playerAttackSoundscapeSource.isPlaying)
        {
            {
                playerAttackSoundscapeSource.clip = fireAttack;
                playerAttackSoundscapeSource.Play();
            }
        }
    }
    internal void StopAttackSoundscape()
    {
        playerAttackSoundscapeSource.Stop();
    }
}
