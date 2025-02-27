using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyHealth))]
public class Enemy_Boss : MonoBehaviour
{
    EnemyHealth eh;

    private void Awake()
    {
        eh = GetComponent<EnemyHealth>();
 
        eh.maxHp = EnemyWaveSpawner.Instance.GetWaveIndex() * 400;
        // mit der maxHp wird dann in EnemyHealth.Start() die endgueltige HP berechnet
    }
}