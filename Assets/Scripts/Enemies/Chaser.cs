using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Chaser hat kein Enemy_TD Script, daher beinhaltet dieses Script die EnemyHandler Interaktionen
/// </summary>


[RequireComponent(typeof(Rigidbody))]
public class Chaser : MonoBehaviour
{
    Rigidbody rb;
    Enemy_TD etd;
    Transform playerTrans;
    Transform targetTrans;

    public GameObject chaserShotPrefab;
    public Transform firePoint;

    public LayerMask targetLayers;
    public float movementSpeed = 25f;
    public float targetingRadius = 25f;

    [Tooltip("Time between single shots in a salve")]
    public float timeBetweenShots = .1f;

    [Tooltip("Delay until next shots")]
    public float shootingDelay = 3f;

    bool shooting;   // for coroutine

    [Tooltip("Delay between updating velocity")]
    public float moveDelay = 1f;
    float moveTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        etd = GetComponent<Enemy_TD>();
        playerTrans = GlobalInfo.Instance.playerTrans;

        etd.canMove = false;
    }

    void Update()
    {
        moveTimer += Time.deltaTime;

        if (moveTimer >= moveDelay)
        {
            ChaserMove();
            moveTimer = 0f;
        }

        if (shooting)
        {
            StartCoroutine(ChaserShots());
        }
    }

    private void FixedUpdate()
    {
        if (shooting)
        {
            Quaternion lookRotation = Quaternion.RotateTowards(rb.rotation, new Quaternion(1f, 1f, 1f, 1f), Time.fixedDeltaTime);
            rb.MoveRotation(lookRotation.normalized);
        }
    }

    private void ChaserMove()
    {
        if (targetTrans != null)
        {
            MoveTowardsTarget();
            shooting = true;
        }
        else
        {
            MoveTowardsBeacon();
            RangeCheck();
        }
    }

    private void MoveTowardsBeacon()
    {
        Vector3 targetPosition = new Vector3(0f, 15f, 0f) + Vector3.up * 5 + new Vector3(Random.Range(-3f, 3f), 0f, Random.Range(-3f, 3f));
        //Debug.Log($"Chaser targetPosition {targetPosition}");
        Vector3 direction = targetPosition - transform.position;
        rb.velocity = movementSpeed * direction.normalized;
    }

    private void MoveTowardsTarget()
    {
        Vector3 targetPosition = playerTrans.position + Random.onUnitSphere + new Vector3(Random.Range(-3f, 3f), 0f, Random.Range(-3f, 3f)) + Vector3.up * 5;
        Vector3 direction = targetPosition - transform.position;
        rb.velocity = movementSpeed * direction.normalized;
    }

    private void RangeCheck()   // Search for target and assign if found
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, targetingRadius, targetLayers, QueryTriggerInteraction.Ignore);

        if (hits.Length > 0)
        {
            targetTrans = hits[0].transform;
        }
    }

    private void CancelMove()
    {
        moveTimer = 0f;
        rb.velocity = Vector3.zero;
    }

    IEnumerator ChaserShots()
    {
        shooting = false;
        ShootPlayer();
        yield return new WaitForSeconds(timeBetweenShots);
        ShootPlayer();
        yield return new WaitForSeconds(timeBetweenShots);
        ShootPlayer();
        yield return new WaitForSeconds(shootingDelay);
        shooting = true;
    }

    private void ShootPlayer()
    {
        CancelMove();
        transform.LookAt(playerTrans);
        GameObject chaserShot = Instantiate(chaserShotPrefab, firePoint.position, Quaternion.identity);
        chaserShot.transform.Rotate(Vector3.up * 90);
    }
}