using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyWaveSpawner))]
public class SpecialWaves : Singleton<SpecialWaves>
{
    EnemyWaveSpawner ews;

    [Header("Prefabs")]
    public GameObject[] waveBossPrefabs = new GameObject[5];
    public GameObject chaserPrefab;
    public GameObject spikeyPrefab;
    public GameObject spacerPrefab;
    public GameObject portlerPrefab;

    [Header("Settings")]
    [Tooltip("Spawns special mobs when the wave number is higher than this value")]
    public int specialMobsWaveThreshold = 5;

    [Tooltip("Amount of special mobs that should be spawned per wave. Wave Index will be added to this.")]
    public int specialMobAmount = 1;

    [Tooltip("First special mob will spawn in x seconds")]
    public float specialMobDelay = 7f;

    [Tooltip("Next special mob will spawn after x seconds")]
    public float nextSpecialMobDelay = 3f;

    GameObject prefab;      // special Mob prefab for Routine

    internal bool isBossWave;

    enum SpecialMob
    {
        Chaser,
        Spikey,
        Spacer,
        Portler
    }
    SpecialMob mob;

    enum BossMob
    {
        WaveBoss
    }
    BossMob boss;

    private void Start()
    {
        ews = GetComponent<EnemyWaveSpawner>();

        ews.onWaveStart += TrySpawnSpecialMobs;
        ews.onWaveSpawnFinished += TrySpawnWaveBoss;
    }

    void TrySpawnWaveBoss()
    {
        if (!EnemyWaveSpawner.Instance.isInWave)
            return;

        int waveIndex = ews.GetWaveIndex();
        int bossIndex = waveIndex / 10 - 1;

        if (waveIndex % 10 == 0)
        {
            StartBossFight(bossIndex);
        }
    }

    private void StartBossFight(int bossIndex)
    {
        Debug.Log($"Boss Fight {bossIndex}");
        GameObject boss = SpawnAndGetMob(waveBossPrefabs[bossIndex]);
        if (boss.TryGetComponent(out EnemyHealth bossEh))
            UIManager.Instance.ShowBossHpBar(bossEh);
    }
    internal void DoEndBossFight()
    {
        StartCoroutine(EndBossRoutine());
    }

    IEnumerator EndBossRoutine()
    {
        for (int i = 0; i < 100; i++)
        {
            SpawnMob(waveBossPrefabs[waveBossPrefabs.Length - 1]);
            yield return new WaitForSeconds(1);
        }
    }

    void TrySpawnSpecialMobs()
    {
        if (!EnemyWaveSpawner.Instance.isInWave)
            return;

        int waveIndex = ews.GetWaveIndex();
        Debug.Log($"TrySpawnSpecialMob() with waveIndex = {waveIndex}");

        if (waveIndex % 10 == 0)
        {
            isBossWave = true;
        }

        if (waveIndex > 30 && PlayerPrefs.GetInt("PortalReady") < 1)
        {
            SpawnMob(portlerPrefab);
            return;
        }

        if (waveIndex < specialMobsWaveThreshold)
            return;

        if (waveIndex % 3 < 1) // == 0. Entspricht also: if durch 3 teilbar
        {
            Debug.Log("Spawn Chaser");
            mob = SpecialMob.Chaser;
        }
        //else
        //{
        //    Debug.Log("Spawn Spikey");
        //    mob = SpecialMob.Spikey;
        //}
        StartCoroutine(SpecialMobSpawnRoutine(specialMobAmount + waveIndex));
    }

    IEnumerator SpecialMobSpawnRoutine(int amount)
    {
        Debug.Log($"Start SpecialMobSpawnRoutine - {amount} {mob}s");

        yield return new WaitForSeconds(specialMobDelay);

        switch (mob)
        {
            case SpecialMob.Chaser:
                Debug.Log("Chaser Spawn");
                prefab = chaserPrefab;
                break;
            case SpecialMob.Spikey:
                Debug.Log("Spikey Spawn");
                prefab = spikeyPrefab;
                break;
            case SpecialMob.Spacer:
                Debug.Log("Spacer Spawn");
                prefab = spacerPrefab;
                break;
            case SpecialMob.Portler:
                Debug.Log("Portler Spawn");
                prefab = portlerPrefab;
                break;
        }
        for (int i = 0; i < amount; i++)
        {
            if (!EnemyWaveSpawner.Instance.waveFinished)
            {
                SpawnMob(prefab);
                yield return new WaitForSeconds(nextSpecialMobDelay);
            }
        }
    }

    internal void RequestSpacer(Transform mobTrans, Vector3 targetPosition)
    {
        StartCoroutine(RequestSpacersRoutine(mobTrans, targetPosition));
    }

    IEnumerator RequestSpacersRoutine(Transform mobTrans, Vector3 targetPosition)
    {
        GameObject spacer = SpawnAndGetMob(spacerPrefab);
        if (spacer.TryGetComponent(out Spacer sp))
        {
            sp.assignedMobTrans = mobTrans;
            sp.targetWaypointPosition = targetPosition;
        }
        yield return new WaitForSeconds(1f);
    }

    // Spawn Enemies at first Waypoint
    private void SpawnMob(GameObject prefab)
    {
        EnemyHandler.Instance.remainingMobs += 1;
        Instantiate(prefab, ews.transform.GetChild(0).GetChild(0).position + Vector3.up * 3f, Quaternion.identity);
    }
    private GameObject SpawnAndGetMob(GameObject prefab)
    {
        EnemyHandler.Instance.remainingMobs += 1;
        return Instantiate(prefab, ews.transform.GetChild(0).GetChild(0).position + Vector3.up * 3f, Quaternion.identity);
    }

    public void FinishSpecialWaves()
    {
        StopAllCoroutines();
        
        Debug.Log("Special Wave Finished");
    }
}
