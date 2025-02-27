using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : TrapPickable, IInteractable
{
    [Tooltip("Prefab das instanziert wird wenn Trap detoniert")]
    public GameObject activationEffectPrefab;

    [Tooltip("Particles wenn Trap geladen ist")]
    public GameObject trapChargedParticles;

    public GameObject rangeIndicatorSphere;

    bool trapCharged;

    public float checkForEnemiesDelay = .5f;
    public float checkRadius = 15f;
    float checkTimer = 0f;

    internal int trapIndex = 0;     // index of this trap object

    public enum TrapType
    {
        Water,
        Fire,
        Air,
        Earth,
        Time
    }
    public TrapType trapType;

    private void Awake()
    {
        TrapManager.Instance.TryAddTrap(this);
    }

    private void Update()
    {
        if (trapCharged)
        {
            checkTimer += Time.deltaTime;
            if (checkTimer > checkForEnemiesDelay)
            {
                checkTimer = 0f;
                Collider[] hits = Physics.OverlapSphere(transform.position, checkRadius, GlobalInfo.Instance.enemyLayer, QueryTriggerInteraction.UseGlobal);
                if (hits.Length > 0)
                {
                    ActivateTrap();
                }
            }
        }
    }

    public void ToggleTrap()
    {
        trapCharged = !trapCharged;
        trapChargedParticles.SetActive(trapCharged);
        rangeIndicatorSphere.SetActive(trapCharged);

        SoundManager.Instance.PlaySoundAt(SoundManager.Sound.Interact, transform.position);
    }

    public void Interact() => ToggleTrap();
    public virtual void ActivateTrap()
    {
        Debug.Log($"{gameObject} activated!");
        TrapManager.Instance.UnsaveTrap(this);
        Instantiate(activationEffectPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        if (trapCharged)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, checkRadius);
        }
    }
}