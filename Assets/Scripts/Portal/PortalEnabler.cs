using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalEnabler : MonoBehaviour
{
    public GameObject portalInside;
    Animator anim;

    void OnEnable()
    {
        if (EnemyWaveSpawner.Instance == null) // wenn keine Main Scene aktiv ist
        {
            enabled = false;
            return;
        }

        EnemyWaveSpawner.Instance.onWaveStart += TogglePortal;
        EnemyWaveSpawner.Instance.onWaveFinished += TogglePortal;
    }

    private void Start()
    {
        anim = GetComponent<Animator>();

        if (!PlayerPrefs.HasKey("PortalReady") || PlayerPrefs.GetInt("PortalReady") < 1)
        {
            gameObject.SetActive(false);
        }
    }

    void TogglePortal()
    {
        Debug.Log("TogglePortal");
        portalInside.SetActive(!portalInside.activeSelf);
    }

    private void OnDisable()
    {
        if (EnemyWaveSpawner.Instance != null)
        {
            EnemyWaveSpawner.Instance.onWaveFinished -= TogglePortal;
            EnemyWaveSpawner.Instance.onWaveStart -= TogglePortal;
        }
    }
}