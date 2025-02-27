using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LighthouseBeam))]
public class DefenseTowerShooting : Singleton<DefenseTowerShooting>
{
    [Header("References")]
    [Tooltip("Position where projectile will be instantiated")]
    public Transform firePoint;
    [Tooltip("LineRenderer for Laser Beams")]
    public LineRenderer lr;
    [Tooltip("Lighthouse Beam Pivot Rotation Script")]
    public RotateLighthouseBeamPivot lbp;


    [Header("Properties")]
    [Tooltip("Time until next shot")]
    public float shootingDelay = 1f;
    public float checkForEnemiesDelay = .1f;
    float checkTimer = 0f;

    public LayerMask enemyLayer;

    private float laserTimer = 0f;

    private void Start()
    {
        ResetLaser();

        if (GlobalInfo.Instance == null)
        {
            enabled = false;
            return;
        }
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
        if (checkTimer > checkForEnemiesDelay)
        {
            checkTimer = 0f;
            TryShootEnemies();
        }
    }

    private void ResetLaser()
    {
        lr.SetPositions(new Vector3[] {
            firePoint.position,
            firePoint.position
        });
    }

    private void TryShootEnemies()
    {
        if (LighthouseManager.Instance.lighthouseDmg < 1)
        {
            return;
        }

        Collider[] hits = Physics.OverlapSphere(transform.position, LighthouseManager.Instance.lighthouseRange, enemyLayer, QueryTriggerInteraction.Ignore);

        if (hits.Length > 0)
        {
            ShootEnemy(hits[0].transform);
        }
    }

    private void ShootEnemy(Transform enemyTrans)
    {
        laserTimer = 0f;

        transform.LookAt(enemyTrans, Vector3.up);
        lr.SetPosition(1, enemyTrans.position);
        //lbp.SetLookAt(enemyTrans);
        

        if (enemyTrans.TryGetComponent(out EnemyHealth enemyHealthComponent))
        {
            enemyHealthComponent.ApplyDamage(LighthouseManager.Instance.lighthouseDmg, Damage.DamageType.Lighthouse);
        }
        else
            Debug.Log($"No EnemyHealth component found on {enemyTrans}");
    }
}
