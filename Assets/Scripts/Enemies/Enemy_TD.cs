using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(EnemyHealth))]
public class Enemy_TD : MonoBehaviour
{
    Rigidbody rb;
    EnemyHealth eh;

    public GameObject nexusDamageParticles;

    [Tooltip("When distance to waypoint is lower than this value, next waypoint is targeted")]
    public float wayPointRange = 10f;

    public float invadeRange = 7f;

    [Tooltip("How much damage to deal to lighthouse on invasion")]
    public int lighthouseDamage = 1;

    [Tooltip("AddForce() Speed")]
    public float startSpeed = 10f;
    [Tooltip("Speed Multiplier for the way from last normal waypoint to end position")]
    public float invadeSpeedMultiplier = 1.5f;
    [Tooltip("Rb velocity multiplier")]
    public float velocitySpeed = 10f;
    [Tooltip("Max rb velocity Speed ^2")]
    public float sqrMaxVelocity = 1000f;
    public enum WaypointPath
    {
        WayR, WayL, Direct
    }
    public WaypointPath waypointPath;
    private Transform[] wayPoints;

    internal float speed;
    Transform target;
    int waypointIndex = 0;

    internal float timerSinceWaypoint = 0f;
    float maxWaypointTime = 20f;    // time in seconds until mob will call a spacer
    internal bool spacerRequested;

    internal bool canMove { get; set; }

    private void OnEnable()
    {
        eh = GetComponent<EnemyHealth>();
        eh.onDeath += TryFinishWave;
    }

    private void Start()
    {
        EnemyHandler.Instance.AddEnemy(gameObject);

        rb = GetComponent<Rigidbody>();

        AssignWaypoints();

        speed = startSpeed;

        canMove = true;
    }

    private void AssignWaypoints()
    {
        if (waypointPath.Equals(WaypointPath.WayR))
            wayPoints = WayPoints.rPoints;  
        else if (waypointPath.Equals(WaypointPath.WayL))
            wayPoints = WayPoints.lPoints;      
        else if (waypointPath.Equals(WaypointPath.Direct))
            wayPoints = WayPoints.dPoints;

        target = wayPoints[0];
    }

    private void FixedUpdate()
    {
        timerSinceWaypoint += Time.fixedDeltaTime;

        if (timerSinceWaypoint > maxWaypointTime)
            TryRequestSpacer();

        else if (canMove)
        {
            Move();
        }
        
        CheckWaypointAndTryMove();
    }

    private void TryRequestSpacer()
    {
        if (transform.position.y < -50f)
        {
            eh.Die();
            return;
        }

        if (spacerRequested)
            return;

        timerSinceWaypoint = 0f;

        SpecialWaves.Instance.RequestSpacer(transform, target.position);
        spacerRequested = true;
        canMove = false;
    }

    private void Move()
    {
        Vector3 dir = target.position - transform.position;
        if (rb.velocity.sqrMagnitude < sqrMaxVelocity)
            rb.AddForce(dir.normalized * speed, ForceMode.Force);
    }
    private void CheckWaypointAndTryMove()
    {
        float distanceToWaypoint = Vector3.Distance(transform.position, target.position);
        if (canMove && waypointIndex > (wayPoints.Length - 3) && distanceToWaypoint < wayPointRange * 1.7f)    // bei den letzten beiden Waypoints geben sich die Mobs nochmal extra Muehe
        {
            MoveTowardsWaypointByVelocity();
        }

        if (distanceToWaypoint <= wayPointRange)
        {
            timerSinceWaypoint = 0f;
            GetNextWayPoint();
        }
    }

    private void MoveTowardsWaypointByVelocity()
    {
        if (rb.velocity.sqrMagnitude < sqrMaxVelocity)
            rb.velocity = (target.position - transform.position).normalized * velocitySpeed;
    }

    private void GetNextWayPoint()
    {
        if (waypointIndex == wayPoints.Length - 2)
        {
            Debug.Log("Last normal Waypoint");
            wayPointRange = invadeRange;
            speed *= invadeSpeedMultiplier;
        }

        if (waypointIndex >= wayPoints.Length - 1)
        {
            InvadeLighthouse();
        }
        else
        {
            waypointIndex++;
            target = wayPoints[waypointIndex];
        }
    }

    private void InvadeLighthouse()
    {
        Instantiate(nexusDamageParticles, transform.position, Quaternion.identity);
        LighthouseManager.Instance.ApplyDamageToLighthouse(lighthouseDamage);
        TryFinishWave();
        eh.Die();
    }

    internal void ResetWayPointIndex()
    {
        waypointIndex = 0;
        GetNextWayPoint();
    }

    private void TryFinishWave()
    {
        EnemyHandler.Instance.TryFinishWave();
    }

    private void OnDisable()
    {
        if (eh != null)
            eh.onDeath -= TryFinishWave;
    }

}
