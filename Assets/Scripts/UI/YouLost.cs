using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YouLost : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 0;
        EnemyWaveSpawner.Instance.allowReady = false;
        MusicManager.Instance.PlayMusic(MusicManager.Instance.splashScreenMusic);
        GameController.StopGameLogic();
    }

    private void OnDisable()
    {
        if (UIManager.Instance != null)
            UIManager.Instance.inLostScreen = false;

        GameController.StartGameLogic();
        Time.timeScale = 1;
    }
}