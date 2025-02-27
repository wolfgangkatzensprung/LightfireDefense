using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAllowReady : MonoBehaviour
{
    void Awake()
    {
        if (GlobalInfo.Instance == null) // wenn keine Main Scene aktiv ist
        {
            enabled = false;
            return;
        }

        EnemyWaveSpawner.Instance.allowReady = true;
    }
}
