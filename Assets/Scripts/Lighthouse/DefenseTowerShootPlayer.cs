using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseTowerShootPlayer : MonoBehaviour
{
    [Header("References")]
    Rigidbody rb;

    [Tooltip("Position where projectile will be instantiated")]
    public Transform firePoint;
    [Tooltip("LineRenderer for Laser Beams")]
    public LineRenderer lr;

    [Header("Settings")]
    [Tooltip("Y value of player where RedEye starts shooting")]
    public float maxHeight = 50f;

    [Tooltip("Time until next shot")]
    public float shootingDelay = 1f;
    public float checkDelay = .1f;
    float checkTimer = 0f;

    private float laserTimer = 0f;

    public float pullForce = 50f;

    private void Start()
    {
        ResetLaser();

        if (GlobalInfo.Instance == null) // wenn keine Main Scene aktiv ist
        {
            enabled = false;
            return;
        }

        rb = GlobalInfo.Instance.playerRb;
    }

    private void Update()
    {
        laserTimer += Time.deltaTime;
        if (laserTimer > shootingDelay)
        {
            ResetLaser();
            laserTimer = 0f;
        }

        checkTimer += Time.deltaTime;
        if (checkTimer > checkDelay)
        {
            checkTimer = 0f;
            TryShootPlayer();
        }
    }

    private void ResetLaser()
    {
        lr.SetPositions(new Vector3[] {
            firePoint.position,
            firePoint.position
        });
    }

    private void TryShootPlayer()
    {
        if (GlobalInfo.Instance.playerTrans.position.y > maxHeight)
        {
            ShootPlayer();
        }
    }

    private void ShootPlayer()
    {
        Transform playerTrans = GlobalInfo.Instance.playerTrans;
        laserTimer = 0f;

        transform.LookAt(playerTrans, Vector3.up);
        lr.SetPosition(1, playerTrans.position + Vector3.up);

        Vector3 direction = (Vector3.zero - playerTrans.position).normalized;
        rb.AddForce(direction * pullForce);
    }
}