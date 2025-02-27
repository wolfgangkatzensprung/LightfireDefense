using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCubes : MonoBehaviour
{
    private Transform playerTrans;

    [Tooltip("Prefabs of Aggro Stomping Cubes")]
    public GameObject[] cubePrefabs;

    [Tooltip("When player y is below this value, Cubes will be spawned")]
    public float yThreshold = -30f;

    private void Start()
    {
        if (GlobalInfo.Instance != null)
        {
            playerTrans = GlobalInfo.Instance.playerTrans;
        }
    }

    private void Update()
    {
        if (playerTrans?.position.y < yThreshold)
        {
            SpawnTheCubes();
            Destroy(gameObject);
        }
    }

    private void SpawnTheCubes()
    {
        for (int i = 0; i < cubePrefabs.Length; i++)
        {
            GameObject cube = Instantiate(cubePrefabs[i], transform.GetChild(i).position, Quaternion.identity);
            if (cube.TryGetComponent(out StompingCubeMovement scm))
                scm.isAggro = true;
        }
    }
}