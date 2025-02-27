using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(EnemyHealth))]
public class Spacer : MonoBehaviour
{
    Rigidbody rb;
    EnemyHealth eh;

    [Tooltip("AddForce Speed (ForceMode.VelocityChange")]
    public float forceStrength = 1f;
    [Tooltip("MoveSpeed that is applied when Rigidbody turns to Kinematic")]
    public float moveSpeed = 1f;

    internal Transform assignedMobTrans;
    internal Vector3 targetWaypointPosition;
    private Vector3 startPos;

    float gatherTimer = 0f;
    float maxGatherTime = 15f;

    internal enum SpacerState
    {
        Gather,     // zum Sammelpunkt
        Pickup,     // zum Mob
        Carry,      // mit Mob zum Sammelpunkt
        Waypoint,   // mit Mob zum Waypoint
        Return      // zurueck zum Portal
    }
    internal SpacerState state;

    private void Start()
    {
        EnemyHandler.Instance.AddEnemy(gameObject);

        rb = GetComponent<Rigidbody>();
        eh = GetComponent<EnemyHealth>();

        eh.onDeath += ReleaseMobAndTryFinishWave;

        startPos = transform.position;
    }

    void FixedUpdate()
    {
        HandleState();
    }

    private void HandleState()
    {
        switch (state)
        {
            case SpacerState.Gather:    // not kinetic
                gatherTimer += Time.deltaTime;
                if (gatherTimer > maxGatherTime)
                {
                    gatherTimer = 0;
                    GotoReturnState();
                    return;
                }
                MoveToSammelpunkt();
                break;

            case SpacerState.Pickup:    // kinetic
                MoveToMob();
                break;

            case SpacerState.Carry:     // kinetic
                MoveToSammelpunktWithMob();
                break;

            case SpacerState.Waypoint:  // kinetic
                MoveToWaypointWithMob();
                break;

            case SpacerState.Return:    // kinetic
                ReturnToPortal();
                break;
        }
    }

    internal void GotoPickupState()
    {
        rb.isKinematic = true;
        state = SpacerState.Pickup;
    }

    internal void GotoGatherState()
    {
        rb.isKinematic = false;
        state = SpacerState.Gather;
    }

    internal void GotoReturnState()
    {
        rb.isKinematic = true;
        state = SpacerState.Return;
    }

    private void MoveToSammelpunkt()
    {
        Vector3 direction = (SpacerSammelpunkt.sammelPunkt - transform.position).normalized;
        rb.AddForce(direction * forceStrength, ForceMode.VelocityChange);
    }
    
    private void MoveToMob()
    {
        if (assignedMobTrans != null)
        {
            Vector3 direction = ((assignedMobTrans.position + Vector3.up * 5) - transform.position).normalized;
            rb.MovePosition(transform.position + direction * Time.deltaTime * moveSpeed);
            if (Vector3.Distance(transform.position, assignedMobTrans.position) < 6f)
            {
                PickupMob();
            }
        }
        else
        {
            GotoGatherState();
        }
    }

    private void MoveToSammelpunktWithMob()
    {
        Vector3 direction = (SpacerSammelpunkt.sammelPunkt - transform.position).normalized;
        rb.MovePosition(transform.position + direction * Time.deltaTime * moveSpeed * 2);
    }

    private void MoveToWaypointWithMob()
    {
        Vector3 direction = (targetWaypointPosition + Vector3.up * 5f - transform.position).normalized;
        rb.MovePosition(transform.position + direction * Time.deltaTime * moveSpeed);

        if (Vector3.Distance(transform.position, targetWaypointPosition) < 6f)
        {
            DropMobAtWaypoint();
        }
    }

    private void ReturnToPortal()
    {
        Vector3 direction = (startPos - transform.position).normalized;
        rb.MovePosition(transform.position + direction * Time.deltaTime * moveSpeed * 2);

        if (Vector3.Distance(transform.position, startPos) < 6f)
        {
            if (TryGetComponent(out Enemy_DropLoot lootComponent))
            {
                lootComponent.exp = 0;
                lootComponent.lootDropChance = 0;
            }
            if(eh.TryGetComponent(out Enemy_ParticlesOnDamaged epd))
            {
                epd.enabled = false;
            }
            eh.Die();
        }
    }
    private void PickupMob()
    {
        if (assignedMobTrans.TryGetComponent(out Rigidbody mobRb))
        {
            mobRb.isKinematic = true;
            mobRb.velocity = Vector3.zero;
        }
        if (assignedMobTrans.TryGetComponent(out Enemy_SpellEffector spellEffector))
        {
            spellEffector.immune = true;
        }
        //if (assignedMobTrans.TryGetComponent(out Spikey spik))
        //{
        //    spik.canMove = false;
        //}

        assignedMobTrans.SetParent(transform);
        state = SpacerState.Carry;
    }
    private void DropMobAtWaypoint()
    {
        DropMob();
        GotoGatherState();
    }

    private void ReleaseMobAndTryFinishWave()  // called durch eh.onDeath delegate
    {
        if (assignedMobTrans != null)
        {
            SpacerSammelpunkt.unassignedStuckMobs.Add(assignedMobTrans);

            eh.onDeath -= ReleaseMobAndTryFinishWave;
            DropMob();
            GotoGatherState();
        }
        
        EnemyHandler.Instance.TryFinishWave();
    }

    private void DropMob()
    {
        if (assignedMobTrans != null && assignedMobTrans.IsChildOf(transform))
        {
            assignedMobTrans.SetParent(null);

            if (assignedMobTrans.TryGetComponent(out Enemy_TD etd))
            {
                etd.canMove = true;
                etd.timerSinceWaypoint = 0f;
                etd.spacerRequested = false;
                //if (assignedMobTrans.TryGetComponent(out Spikey spik))
                //{
                //    spik.canMove = true;
                //    etd.canMove = false;
                //}
            }
            if (assignedMobTrans.TryGetComponent(out Enemy_SpellEffector spellEffector))
            {
                spellEffector.immune = false;
            }
            if (assignedMobTrans.TryGetComponent(out Rigidbody mobRb))
            {
                mobRb.isKinematic = false;
            }

            assignedMobTrans = null;
        }
    }

    private void OnEnable()
    {
        SpacerSammelpunkt.spacers.Add(transform);
    }

    private void OnDisable()
    {
        DropMob();
        SpacerSammelpunkt.spacers.Remove(transform);
    }
}
