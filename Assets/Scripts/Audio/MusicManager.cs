using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : Singleton<MusicManager>
{
    [Tooltip("Master Audio Channel")]
    public AudioMixer master;

    public AudioClip menuMusic;
    public AudioClip splashScreenMusic;

    public AudioClip lighthouseIdle;
    public AudioClip lighthouseFight;

    public AudioClip waveFinishedJingle;
    public AudioClip deathJingle;
    public AudioClip lostJingle;

    public AudioClip fluxCube;
    public AudioClip enemyWorld;

    public AudioClip forest;
    public AudioClip sky;
    public AudioClip archipel;
    public AudioClip lavaLake;

    public AudioClip arcane;

    AudioSource source;

    bool lowpassLerp; // cutoff/lowpass fade
    float lpLerpTimer = 0f; // lowpass lerp t
    float lpLerpSpeed = 3f;
    float currentCutoffFrequency;

    bool transitioning; // volume fade
    AudioMixerSnapshot snapshot;
    [Tooltip("Time to transition to snapshot")]
    public float snapshotTransitionTime = .3f;
    float snapshotTransitionTimer = 0f;

    // Time to wait after transition until starting to play jingle
    float snapshotHoldingTime = 1f;

    // if Jingle is being played
    bool playingJingle;
    Coroutine jingleRoutine;

    [Tooltip("Modulation speed of spooky music")]
    public float spookySpeed = .3f;
    private float spookyTimer = 0f;
    //private const float bossPitch = .840896151f;    // D wird zu H
    private const float bossPitch = .749153456f;    // D wird zu A => ca 18 Sekunden bis Gitarreneinsatz

    public override void Awaken()
    {
        source = GetComponent<AudioSource>();
    }

    private void Start()
    {
        PlayerHealth.Instance.onDeath += PlayDeathJingle;
        EnemyWaveSpawner.Instance.onWaveFinished += PlayWaveFinishedJingle;
        EnemyWaveSpawner.Instance.onWaveStart += PlayFightMusic;
        SceneLoading.Instance.onSceneLoadedAsync += PlayMusicByScene;
    }

    private void Update()
    {
        if (transitioning && snapshotTransitionTimer < snapshotTransitionTime)
        {
            snapshotTransitionTimer += Time.deltaTime;
            TransitionToSnapshot(snapshot, snapshotTransitionTime);
        }
        else
        {
            transitioning = false;
            snapshotTransitionTimer = 0f;
        }

        if (EnemyWaveSpawner.Instance.isTDlevel && !GlobalInfo.inIntro)
            HandlePitchChanges();

        if (lowpassLerp)
            DoLowpassLerp();

    }

    private void DoLowpassLerp()
    {
        if (UIManager.Instance.inMenu)
        {
            master.GetFloat("Cutoff", out currentCutoffFrequency);
            master.SetFloat("Cutoff", Mathf.Lerp(currentCutoffFrequency, 500f, lpLerpTimer));
            //Debug.Log($"Lerping Cutoff with currentCutoffFreq {currentCutoffFrequency} and lerpTimer {lpLerpTimer}");
        }
        else
        {
            master.GetFloat("Cutoff", out currentCutoffFrequency);
            master.SetFloat("Cutoff", Mathf.Lerp(currentCutoffFrequency, 22000f, lpLerpTimer));
        }

        lpLerpTimer += Time.unscaledDeltaTime * lpLerpSpeed;
        if (lpLerpTimer > 1f)
        {
            lowpassLerp = false;
            lpLerpTimer = 0f;
        }

    }

    private void HandlePitchChanges()
    {
        if (!LightRadiusHandler.playerInside)
        {
            spookyTimer += Time.deltaTime;
            source.pitch = Mathf.Lerp(source.pitch, Mathf.PingPong(spookyTimer * spookySpeed, 1f) + .5f + .1f * Mathf.PingPong(spookyTimer, 1f), spookyTimer);
        }
        else if (EnemyWaveSpawner.Instance.isInWave && SpecialWaves.Instance.isBossWave)
        {
            SetBossPitch();
        }
        else if (LightRadiusHandler.playerInside)
        {
            spookyTimer = 0f;
            if (SpecialWaves.Instance.isBossWave)
            {
                SetBossPitch();
            }
            else
            {
                TryResetPitch();
            }
        }
    }

    private void SetBossPitch()
    {
        Debug.Log("Boss Pitch");
        source.pitch = bossPitch;
    }
    private void TryResetPitch()
    {
        if (source.pitch != 1)
        {
            //Debug.Log("Reset Pitch to normal");
            source.pitch = 1f;
        }
    }

    public void PlayIntroMusic()
    {
        Debug.Log("Intro Music");
        PlayMusic(splashScreenMusic);
    }

    public void PlayIdleMusic()
    {
        Debug.Log("Play IdleMusic");

        if (playingJingle)
            return;

        PlayMusic(lighthouseIdle);
    }

    public void PlayFightMusic()
    {
        StopAllCoroutines();

        if (SpecialWaves.Instance.isBossWave)
            SetBossPitch();

        PlayMusic(lighthouseFight);
    }

    public void PlayMusic(AudioClip soundTrack)
    {
        ResetSnapshot();
        source.loop = true;
        source.clip = soundTrack;
        source.Play();
    }

    public void PlayJingle(AudioClip soundTrack)
    {
        playingJingle = true;
        source.loop = false;
        source.clip = soundTrack;
        source.Play();
    }

    public void PlayWaveFinishedJingle()
    {
        if (SpecialWaves.Instance.isBossWave)
            TryResetPitch();

        if (GlobalInfo.inMenu)
        {
            PlayMusic(menuMusic);
            return;
        }

        snapshot = master.FindSnapshot("FadeOut");
        snapshotTransitionTimer = 0f;
        transitioning = true;

        jingleRoutine = StartCoroutine(PlayJingleRoutineThenContinue(waveFinishedJingle));
    }

    private void PlayDeathJingle()
    {
        snapshot = master.FindSnapshot("FadeOut");
        snapshotTransitionTimer = 0f;
        transitioning = true;
        jingleRoutine = StartCoroutine(PlayJingleRoutineThenContinue(deathJingle));
    }

    IEnumerator PlayJingleRoutineThenContinue(AudioClip jingleClip)
    {
        playingJingle = true;
        Debug.Log("Jingle - start coroutine");
        yield return new WaitForSeconds(snapshotTransitionTime + snapshotHoldingTime);
        Debug.Log("Jingle - play jingle music");
        ResetSnapshotWhileInRoutine();
        PlayJingle(jingleClip);
        Debug.Log("jingle - length: "+ source.clip.length);
        yield return new WaitForSeconds(source.clip.length);
        Debug.Log("Jingle - end jingle music");
        playingJingle = false;
        PlayIdleMusic();
    }

    private void TransitionToSnapshot(AudioMixerSnapshot snapshot, float transitionTime)
    {
        snapshot.TransitionTo(transitionTime);
    }

    public void ResetSnapshot()
    {
        if (jingleRoutine != null && playingJingle)
        {
            Debug.Log("Jingle - routines stopped");
            StopCoroutine(jingleRoutine);
        }
        transitioning = false;
        master.FindSnapshot("Main").TransitionTo(0);
    }
    public void ResetSnapshotWhileInRoutine()
    {
        transitioning = false;
        master.FindSnapshot("Main").TransitionTo(0);
    }

    public void ApplyMasterLowpass()
    {
        lowpassLerp = true;
        lpLerpTimer = 0f;
    }
    public void ResetMasterLowpass()
    {
        lowpassLerp = true;
        lpLerpTimer = 0f;
    }

    public void PlayMusicByScene(string sceneName)
    {
        if (playingJingle)
        {
            ResetSnapshot();
            source.volume = PlayerPrefs.GetFloat("MusicVolume");
        }

        StopAllCoroutines();

        switch (sceneName)
        {
            case "TD Level":
                if (GlobalInfo.isNewStart)
                    PlayIntroMusic();
                else
                    PlayMusic(lighthouseIdle);
                break;
            case "FluxCube":
                PlayMusic(fluxCube);
                break;
            case "Forest":
                PlayMusic(forest);
                break;
            case "Sky":
                PlayMusic(sky);
                break;
            case "Archipel":
                PlayMusic(archipel);
                break;
            case "LavaLake":
                PlayMusic(lavaLake);
                break;
            case "EnemyWorld":
                PlayMusic(enemyWorld);
                break;
            case "Arcane":
                PlayMusic(arcane);
                break;
        }
    }

    public void SetVolume(float musicVolume)
    {
        source.volume = musicVolume;
    }
}