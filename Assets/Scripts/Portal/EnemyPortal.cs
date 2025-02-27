using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPortal : Portal
{
    Animator anim;

    private void OnEnable()
    {
        if (EnemyWaveSpawner.Instance == null) // wenn keine Main Scene aktiv ist
        {
            enabled = false;
            return;
        }

        anim = GetComponent<Animator>();
        EnemyWaveSpawner.Instance.onMobSpawn += PlayPortalAnim;
    }

    private void PlayPortalAnim()
    {
        anim.Play("EnemyPortalSpawnAnimation");
    }

    private void OnDisable()
    {
#if UNITY_EDITOR
        if (EnemyWaveSpawner.Instance != null)
#endif
        EnemyWaveSpawner.Instance.onMobSpawn -= PlayPortalAnim;
    }
}
