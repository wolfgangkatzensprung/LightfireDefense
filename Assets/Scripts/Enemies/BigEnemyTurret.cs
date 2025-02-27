using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BigEnemyTurret : Enemy_ShootAtPlayer
{
    public GameObject[] projectilePrefabs;

    private void Update()
    {
        if (playerTrans.position.y > minShootingHeight)
        {
            int random = Random.Range(0, projectilePrefabs.Length);
            //Debug.Log($"Shoot {projectilePrefabs[random].name}");
            projectilePrefab = projectilePrefabs[random];
            TryShootPlayer();
        }
    }
}