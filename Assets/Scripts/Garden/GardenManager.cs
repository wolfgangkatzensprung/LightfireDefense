using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GardenManager : Singleton<GardenManager>
{
    public static List<BerryBush> berryBushes = new List<BerryBush>();
    internal int berryBushesAmount = 0;
    public List<GameObject> berries = new List<GameObject>();
    internal Transform currentTargetBerryTrans;

    [Tooltip("Prefab of Particles that play when Berries are fully grown.")]
    public GameObject berrySuckParticlesPrefab;
    private ParticleSystem suckParticles;
    [Tooltip("Prefab of Berry Bush to spawn")]
    public GameObject berryBushPrefab;
    [Tooltip("Seed Prefab")]
    public GameObject seedPrefab;

    public delegate void GardenGrowDelegate();
    public GardenGrowDelegate onGardenGrown;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("BushAmount"))
            PlayerPrefs.SetInt("BushAmount", 0);

        EnemyWaveSpawner.Instance.onWaveFinished += GrowGarden;
        EnemyWaveSpawner.Instance.onWaveFinished += TrySpawnSeed;
    }

    //private void Update()
    //{
    //    // for debug
    //    if (Input.GetKeyDown(KeyCode.G))
    //    {
    //        GrowGarden();
    //    }
    //}

    private void GrowGarden()
    {
        if (berryBushes != null)
        {
            foreach (BerryBush bb in berryBushes)
            {
                if (bb != null)
                    bb.GrowBerries();
            }
        }

        onGardenGrown?.Invoke();
    }

    private void TrySpawnSeed()
    {
        if (EnemyWaveSpawner.Instance.waveNumber > 1 && EnemyWaveSpawner.Instance.waveNumber % 3 != 0)
        {
            return;
        }

        Debug.Log("Spawn MagicSeed");

        float radius = LighthouseManager.Instance.lighthouseRange;
        float x = Random.Range(-radius, radius);
        float z = Random.Range(-radius, radius);
        Vector3 spawnPos = new Vector3(x, 50f, z);

        GameObject seed = Instantiate(seedPrefab, spawnPos, Quaternion.identity);
    }

    internal int AddBerryBush(BerryBush berryBush)
    {
        if (berryBushes == null)
            berryBushes = new List<BerryBush>();

        int bushIndex = berryBushesAmount;

        berryBushes.Add(berryBush);
        berryBushesAmount += 1;

        return bushIndex;
    }

    internal static void DeleteBerryBushObjects()
    {
        BerryBush[] bushs = FindObjectsOfType<BerryBush>();
        foreach (BerryBush bb in bushs)
        {
            berryBushes.Remove(bb);
            Destroy(bb.gameObject);
        }
        berryBushes = null;
    }

    internal static void DeleteBerryBushPrefs()
    {
        if (PlayerPrefs.HasKey("BushAmount"))
            PlayerPrefs.DeleteKey("BushAmount");

        for (int i = 0; i < 15; i++)
        {
            if (PlayerPrefs.HasKey($"Bush{i}x"))
            {
                PlayerPrefs.DeleteKey($"Bush{i}x");
                PlayerPrefs.DeleteKey($"Bush{i}y");
                PlayerPrefs.DeleteKey($"Bush{i}z");
            }
        }

        if (GlobalInfo.Instance.playerTrans.gameObject.TryGetComponent(out PlayerBuild pb))
        {
            pb.ResetBuildingUI();
        }
    }

    internal void AddBerry(GameObject berry)
    {
        berries.Add(berry);
        AssignCurrentBerry(berry.transform);
    }

    internal void RemoveBerry(GameObject berry)
    {
        berries.Remove(berry);
        if (berries.Count != 0)
            AssignCurrentBerry(berries[0].transform);
    }

    // Current Target Berry for LuckySprites
    internal void AssignCurrentBerry(Transform berryTrans)
    {
        currentTargetBerryTrans = berryTrans;
    }

    internal void PlaySuckParticles()
    {
        if (suckParticles == null)
        {
            suckParticles = Instantiate(berrySuckParticlesPrefab, Vector3.zero, Quaternion.identity).GetComponent<ParticleSystem>();
        }
        suckParticles.Play();
    }

    public void SaveBush(BerryBush bb)
    {
        if (SceneLoading.Instance.currentLevelScene.name != "TD Level")
            return;

        Debug.Log("Save BerryBush " + bb);

        int i = bb.bushIndex;

        PlayerPrefs.SetFloat($"Bush{i}x", bb.transform.position.x);
        PlayerPrefs.SetFloat($"Bush{i}y", bb.transform.position.y);
        PlayerPrefs.SetFloat($"Bush{i}z", bb.transform.position.z);

        PlayerPrefs.SetFloat($"Bush{i}BerryScale", bb.currentScale);

        PlayerPrefs.SetInt("BushAmount", berryBushesAmount);

        Debug.Log($"BushPosition {i} saved with BerryScale {bb.currentScale}");
    }

    /// <summary>
    /// Load Bushes from PlayerPrefs and spawn them
    /// </summary>
    public void LoadBushes()
    {
        int amount = PlayerPrefs.GetInt("BushAmount");

        if (amount == 0)
        {
            Instantiate(seedPrefab, new Vector3(10, 3, 7), Quaternion.identity);
        }

        for (int i = 0; i < amount; i++)
        {
            if (!PlayerPrefs.HasKey($"Bush{i}x"))
                continue;

            Vector3 pos = new Vector3(PlayerPrefs.GetFloat($"Bush{i}x"), PlayerPrefs.GetFloat($"Bush{i}y"), PlayerPrefs.GetFloat($"Bush{i}z"));


            GameObject bush = Instantiate(berryBushPrefab, pos, Quaternion.identity);
            Debug.Log("Load BerryBush " + bush);

            BerryBush bb = bush.GetComponent<BerryBush>();
            bb.bushIndex = i;
            bb.LoadBerryScales(i);
            GameController.MoveObjectToCurrentLevelScene(bush);
        }
    }

    private void OnDestroy()
    {
#if UNITY_EDITOR
        return;
#endif
        EnemyWaveSpawner.Instance.onWaveFinished -= GrowGarden;
    }
}