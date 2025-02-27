using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;


// this script is obsolete

public class LostScreen : MonoBehaviour
{
    public GameObject deathTextObject;

    float startTimer = 3f;

    private void Start()
    {
        EnemyWaveSpawner.Instance.allowReady = false;
        PlayerHealth.Instance.onDeath += ResetStartTimer;
    }

    private void OnEnable()
    {
        Time.timeScale = 0;

        PlayerHealth.Instance.onDeath += DeathScreen;
        MusicManager.Instance.PlayMusic(MusicManager.Instance.splashScreenMusic);
    }

    void Update()
    {
        if (startTimer > 0)
        {
            startTimer -= Time.unscaledDeltaTime;
            return;
        }

        if (!Input.anyKeyDown || UIManager.Instance.inIntro || UIManager.Instance.inLostScreen)
            return;

        Debug.Log("Any Key Press");
        StartPlayGame();
    }

    private void StartPlayGame()
    {
        MusicManager.Instance.PlayMusic(MusicManager.Instance.lighthouseIdle);
        Time.timeScale = 1f;
        UIManager.Instance.inMenu = false;
        GlobalInfo.inMenu = false;
        EnemyWaveSpawner.Instance.allowReady = true;
        Destroy(GameObject.Find("Intro"));
        gameObject.SetActive(false);
        deathTextObject.SetActive(false);

    }

    public void DeathScreen()
    {
        if (LighthouseManager.Instance.currentLighthouseHp <= 0)
            return;

        Debug.Log("Death Screen");
        gameObject.SetActive(true);
        deathTextObject.SetActive(true);
    }

    private void ResetStartTimer()
    {
        startTimer = 3f;
    }

    private void OnDisable()
    {
#if UNITY_EDITOR
        return;
#endif
        PlayerHealth.Instance.onDeath -= DeathScreen;
    }
}