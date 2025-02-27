using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyWaveSpawner : Singleton<EnemyWaveSpawner>
{
    [Header("References")]
    public GameObject[] enemyPrefabs;

    [Header("Settings")]

    [Tooltip("Delay between enemy spawns in seconds")]
    public float delayBetweenEnemies = .1f;

    [Tooltip("Start Delays for wave 1-10")]
    public float[] startDelaysBetweenEnemies = new float[] { 2f, 1f, .5f, .4f, .3f, .2f, .15f };

    [Tooltip("Maximum amount of enemies on the map")]
    public int maxEnemiesAmount = 100;

    [Tooltip("Maximum random offset added to spawn position")]
    public float maxRndOffset = 3f;

    EnemiesData.EnemyType[] currentWaveEnemyTypes;

    internal bool isTDlevel = true;   // if current level is a TD level
    internal bool isInWave;
    internal bool waveFinished = true;

    internal bool allowReady { get; set; }    // if Player is allowed to start wave with the Ready Button (Enter)

    // current wave number
    internal int waveNumber = 1;

    internal float timerSinceWaveStart = 0f;
    const float checkInterval = 45f;    // every this amount of seconds, an out of bounds check will be done 
    float checkThreshold = 0f;          // every time the checkInterval is reached, this threshold increases by corresponding amount for the next check

    public delegate void MobSpawnedDelegate();
    public MobSpawnedDelegate onMobSpawn;

    public delegate void WaveSpawnFinishedDelegate();
    public WaveSpawnFinishedDelegate onWaveSpawnFinished;

    public delegate void WaveFinishedDelegate();
    public WaveFinishedDelegate onWaveFinished;

    public delegate void WaveStartedDelegate();
    public WaveStartedDelegate onWaveStart;

    private void Start()
    {
        checkThreshold = checkInterval;

        PlayerInputManager.Instance.onPlayerRdy += TryStartNextWave;
        SceneLoading.Instance.onSceneLoadedAsync += TrySetupWave;
    }

    private void Update()
    {
        timerSinceWaveStart += Time.deltaTime;

        if (timerSinceWaveStart > checkThreshold)
        {
            EnemyHandler.Instance.TryDoOutOfBoundsCheck();
            checkThreshold += checkInterval;
        }
    }

    private void TrySetupWave(string sceneName)
    {
        if (sceneName != "TD Level")
        {
            StopWave();
        }
        else
        {
            isTDlevel = true;
            isInWave = false;
            allowReady = true;
            waveFinished = false;
        }
    }


    private void TryStartNextWave()
    {
        Debug.Log("Try Spawning Next Wave");
        if (allowReady && isTDlevel && !GlobalInfo.inMenu && EnemyHandler.Instance.GetEnemiesOnMap().Count < 1)
        {
            Debug.Log("Spawning Next Wave");
            SpawnNextWave();
            allowReady = false;
        }
        else
        {
            Debug.Log($"Bools: allowReady {allowReady}, readyForNextWave {allowReady}, isTDlevel {isTDlevel}, GlobalInfo.Instance.inMenu {GlobalInfo.inMenu}, EnemiesCount {EnemyHandler.Instance.GetEnemiesOnMap().Count}");
        }
    }

    private void SpawnNextWave()
    {
        isInWave = true;

        timerSinceWaveStart = 0f;
        checkThreshold = checkInterval;

        int totalEnemiesAmount = EnemiesData.enemiesPerWave[waveNumber - 1];
        StartCoroutine(SpawnWave(totalEnemiesAmount));

        EnemyHandler.Instance.remainingMobs = totalEnemiesAmount;
        waveFinished = false;

        // SetNextSpawnDelay();

        onWaveStart?.Invoke();
    }

    private void SetNextSpawnDelay()
    {
        if (waveNumber - 1 < startDelaysBetweenEnemies.Length)
        {
            delayBetweenEnemies = startDelaysBetweenEnemies[waveNumber - 1];
        }
    }

    IEnumerator SpawnWave(int totalEnemiesAmount)
    {
        currentWaveEnemyTypes = EnemiesData.enemyWaveTypes[waveNumber - 1];
        Debug.Log($"SpawnWave with {totalEnemiesAmount} enemies");
        UIManager.Instance.SetStartNextWaveText(false);

        for (int i = 0; i < totalEnemiesAmount; i++)
        {
            LastEnemyCheck(totalEnemiesAmount, i);

            int randomIndex = GetRandomIndex();

            SpawnEnemyByType(currentWaveEnemyTypes[randomIndex]);
            Debug.Log($"SpawnEnemyByType({currentWaveEnemyTypes[randomIndex]})");

            yield return new WaitForSeconds(delayBetweenEnemies);

            // hier koennte max mob amount limitierer stehen
            // while (mobs auf der map > maxMobAmount)
            // yield return null
        }

        FinishWaveSpawn();
    }

    private static void LastEnemyCheck(int totalEnemiesAmount, int i)
    {
        if (i == totalEnemiesAmount - 1)
        {
            EnemyHandler.Instance.lastEnemySpawned = true;
        }
        else EnemyHandler.Instance.lastEnemySpawned = false;
    }

    internal void TryCancelWaveSpawn()
    {
        Debug.Log("TryCancel Wave Spawn");

        if (waveFinished)
        {
            Debug.Log("Wave Spawn not cancelled cuz is already finished");
            return;
        }
        StopAllCoroutines();
        GlobalInfo.Instance.waveNumber = waveNumber;
        isInWave = false;
        waveFinished = true;
        EnemyHandler.Instance.ClearEnemies();
        Debug.Log("Wave Spawn was cancelled");
    }

    private void FinishWaveSpawn()
    {
        SpecialWaves.Instance.FinishSpecialWaves();

        onWaveSpawnFinished?.Invoke();
        waveFinished = true;
        GlobalInfo.Instance.waveNumber = waveNumber;
    }

    internal void FinishWave()
    {
        if (SpecialWaves.Instance.isBossWave)
            SpecialWaves.Instance.isBossWave = false;

        isInWave = false;

        waveNumber++;

        onWaveFinished?.Invoke();

        StartCoroutine(FinishWaveRoutine());
    }

    IEnumerator FinishWaveRoutine()
    {
        SetWaveNumberUI();

        yield return new WaitForSeconds(.81f);  // muss laenger sein als Fading time des SoundManagers

        allowReady = true;
        UIManager.Instance.SetStartNextWaveText(true);

        Debug.Log("Wave finished");
        SaveSystem.SaveGame();
    }

    private void SpawnEnemyByType(EnemiesData.EnemyType enemyType)
    {
        Vector3 randomOffset = new Vector3(Random.Range(-maxRndOffset, maxRndOffset), Random.Range(0, maxRndOffset), Random.Range(-maxRndOffset, maxRndOffset));
        Vector3 spawnPosition = WayPoints.rPoints[0].position + randomOffset;
        Debug.Log($"enemyType: {enemyType} = {(int)enemyType}");
        GameObject enemy = Instantiate(enemyPrefabs[(int)enemyType], spawnPosition, Quaternion.identity);

        onMobSpawn?.Invoke();
    }

    public void SpawnWaveByNumber(int waveIndex)
    {
        this.waveNumber = waveIndex;
        TryStartNextWave();
    }

    private void SetWaveNumberUI()
    {
        UIManager.Instance.SetWaveIndexText(waveNumber.ToString());
    }

    internal void SetWaveIndex(int wave)
    {
        waveNumber = wave;
        GlobalInfo.Instance.waveNumber = wave;
        SetWaveNumberUI();
    }

    private int GetRandomIndex()
    {
        int randomIndex = 0;

        if (currentWaveEnemyTypes.Length > 0)
        {
            randomIndex = Random.Range(0, EnemiesData.enemyWaveTypes[waveNumber - 1].Length);
            Debug.Log($"Random Enemy Index: {randomIndex}. Btw: enemyWaveTypes[{waveNumber - 1}].Length = {EnemiesData.enemyWaveTypes[waveNumber - 1].Length}");
        }

        return randomIndex;
    }

    public int GetWaveIndex()
    {
        return waveNumber;
    }

    public void StopWave()
    {
        Debug.Log("Wave Stopped");
        isInWave = false;
        SpecialWaves.Instance.FinishSpecialWaves();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, .1f, .1f, .6f);
        Vector3 spawnPos = transform.GetChild(0).GetChild(0).position;
        Gizmos.DrawWireCube(spawnPos, Vector3.one * maxRndOffset * 2f);
    }
}