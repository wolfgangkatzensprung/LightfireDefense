using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_ShootAtPlayer : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Projectile that will be shot")]
    public GameObject projectilePrefab;

    [Tooltip("Position where projectile will be instantiated")]
    public Transform firePoint;

    internal Transform playerTrans;


    [Tooltip("Starts shooting when Player is above this height")]
    public float minShootingHeight = 100f;

    [Tooltip("Time until next shot")]
    public float shootingDelay = 1f;
    float cooldownTimer = 0f;

    private void Start()
    {
        if (GlobalInfo.Instance != null)
            playerTrans = GlobalInfo.Instance.playerTrans;
        else
            enabled = false;
    }
    private void Update()
    {
        if (playerTrans.position.y > minShootingHeight)
        {
            TryShootPlayer();
        }
    }

    internal void TryShootPlayer()
    {
        cooldownTimer += Time.deltaTime;
        if (cooldownTimer > shootingDelay)
        {
            ShootPlayer();
            cooldownTimer = 0f;
        }
    }

    private void ShootPlayer()
    {
        transform.LookAt(playerTrans, Vector3.up);
        Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
    }
}
