using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [Tooltip("Projectiles to shoot")]
    public GameObject[] missilePrefabs;
    [Tooltip("Position where missile is instantiated")]
    public Transform firePoint;

    [Tooltip("Reference of GearWheel for Targeting Mode Switch")]
    public Renderer gearRendererL;
    [Tooltip("Reference of GearWheel for Targeting Mode Switch")]
    public Renderer gearRendererR;
    [Tooltip("Material for Targeting Mode: First")]
    public Material firstMat;
    [Tooltip("Material for Targeting Mode: Last")]
    public Material lastMat;

    Collider[] enemies = new Collider[0];
    Transform targetTrans;
    bool targeting;

    internal int turretNumber = 0;  // from 1 to max number (15)

    [Header("Settings")]
    [Tooltip("Enemy detection range")]
    public float range = 10f;

    float checkTimer = 0f;
    [Tooltip("Time between each enemy check in seconds")]
    public float checkDelay = .5f;

    float shootingTimer = 0f;
    [Tooltip("Time between shots in seconds")]
    public float shotDelay = 1f;

    public enum TargetingMode
    {
        First,
        Last
    }
    public TargetingMode targetingMode;

    public enum TurretElement
    {
        None,
        Water,
        Fire,
        Air,
        Earth
    }
    public TurretElement turretElement;

    private void Update()
    {
        checkTimer += Time.deltaTime;
        if (checkTimer > checkDelay)
        {
            TargetClosestEnemy();
            checkTimer = 0f;
        }

        TargetLock();

        shootingTimer += Time.deltaTime;
        if (shootingTimer > shotDelay)
        {
            shootingTimer = 0f;
            TryShoot();
        }
    }

    private void TargetLock()
    {
        if (targeting)
        {
            transform.LookAt(targetTrans);
        }
    }

    public void TargetClosestEnemy()
    {
        enemies = Physics.OverlapSphere(transform.position, range, GlobalInfo.Instance.enemyLayer);
        if (enemies.Length > 0)
        {
            targeting = true;
            targetTrans = enemies[0].transform;
        }
        else targeting = false;
    }

    internal void SwitchTargetMode()
    {
        if (targetingMode != Turret.TargetingMode.Last)
        {
            targetingMode = Turret.TargetingMode.Last;
            gearRendererR.material = lastMat;
            gearRendererL.material = firstMat;
            gameObject.name = "Turret [Shoot Last]";
        }
        else if (targetingMode != Turret.TargetingMode.First)
        {
            targetingMode = Turret.TargetingMode.First;
            gearRendererR.material = firstMat;
            gearRendererL.material = lastMat;
            gameObject.name = "Turret [Shoot First]";
        }
    }

    public void TryShoot()
    {
        if (targeting)
            Shoot();
    }

    private void Shoot()
    {
        GameObject projectile = Instantiate(missilePrefabs[(int)turretElement], firePoint.position, Quaternion.identity);

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        Vector3 direction = firePoint.forward;
        rb.velocity = projectile.GetComponent<Projectile>().speed * direction;
    }

    public void SaveTurretPosition()
    {
        if (SceneLoading.Instance.currentLevelScene.name != "TD Level")
            return;

        int i = turretNumber - 1;

        PlayerPrefs.SetFloat($"Turret{i}x", transform.position.x);
        PlayerPrefs.SetFloat($"Turret{i}y", transform.position.y);
        PlayerPrefs.SetFloat($"Turret{i}z", transform.position.z);

        Debug.Log("TurretPosition " + i + " saved.");
    }
}
