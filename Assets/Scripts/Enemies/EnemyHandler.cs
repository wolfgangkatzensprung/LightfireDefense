using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : Singleton<EnemyHandler>
{
    public bool lastEnemySpawned { get; set; }

    List<GameObject> enemiesOnMap = new List<GameObject>();

    // current remaining mobs
    internal int remainingMobs { get; set; }

    internal bool allowCompleteWave = true;    // if wave is allowed to be completed. This will be false when clearing mobs by going to menu or through portals

    public delegate void MobDiedDelegate();
    public MobDiedDelegate onMobDied;

    private void Start()
    {
        PlayerHealth.Instance.onDeath += ResetEnemiesList;
    }

    public void AddEnemy(GameObject enemy)
    {
        enemiesOnMap.Add(enemy);
    }

    public void RemoveEnemy(GameObject enemy)
    {
        //Debug.Log($"RemoveEnemy {enemy.name} with instance ID {enemy.GetInstanceID()}");

        enemiesOnMap.Remove(enemy);
        remainingMobs -= 1;

        onMobDied?.Invoke();
    }

    public void TryFinishWave()
    {
        Debug.Log("TryFinishWave");

        foreach (GameObject e in enemiesOnMap)
        {
            if(e != null)
            {
                allowCompleteWave = true;
                Debug.Log($"Enemy {e} remains.");
                DoOutOfBoundsCheck(e);
            }
        }

        if (lastEnemySpawned && enemiesOnMap.Count < 1)
        {
            if (allowCompleteWave)
                EnemyWaveSpawner.Instance.FinishWave();
            else
                allowCompleteWave = true;
        }
    }


    internal void TryDoOutOfBoundsCheck()
    {
        foreach (GameObject e in enemiesOnMap)
        {
            if (e != null)
            {
                Debug.Log($"Enemey {e} remains.");
                DoOutOfBoundsCheck(e);
            }
        }
    }

    private static void DoOutOfBoundsCheck(GameObject e)
    {
        if (!(e.TryGetComponent(out Spacer spacer)))
        {
            if (e.transform.position.y > 1000 || e.transform.position.y < -1000)
            {
                ResetMob(e);
            }
        }
    }

    private static void ResetMob(GameObject e)
    {
        e.transform.position = WayPoints.rPoints[0].position;
        if (e.TryGetComponent(out Enemy_TD etd))
        {
            etd.ResetWayPointIndex();
            // rigidbody anhalten
        }
    }

    public List<GameObject> GetEnemiesOnMap()
    {
        return enemiesOnMap;
    }

    internal int GetRemainingMobsAmount()
    {
        return remainingMobs;
    }

    public void ClearEnemies()
    {
        Debug.Log($"Clearing all {enemiesOnMap.Count} Enemies");

        allowCompleteWave = false;

        GameObject[] enemiesArray = enemiesOnMap.ToArray();

        for (int i = 0; i < enemiesArray.Length; i++)
        {
            if (enemiesArray[i].TryGetComponent(out EnemyHealth eh))
            {
                eh.Die();
            }
        }

        Debug.Log($"Enemies cleared. {enemiesOnMap.Count} remaining.");
        if (enemiesOnMap.Count != 0)
            enemiesOnMap = new List<GameObject>();
        remainingMobs = enemiesOnMap.Count;

        TryUpdateRemainingEnemiesUI();
    }

    private static void TryUpdateRemainingEnemiesUI()
    {
        RemainingEnemiesUI reui = FindObjectOfType<RemainingEnemiesUI>();
        if (reui != null)
        {
            reui.UpdateRemainingEnemiesUI();
        }
    }

    void ResetEnemiesList()
    {
        enemiesOnMap = new List<GameObject>();
        TryUpdateRemainingEnemiesUI();
    }
}
