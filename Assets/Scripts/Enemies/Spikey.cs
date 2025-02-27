using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy_TD))]
public class Spikey : MonoBehaviour
{
    Enemy_TD td;
    Rigidbody rb;

    public float aggroRadius = 15f;

    public float velocitySpeed = 30f;

    bool aggroed;
    [Tooltip("Small knockback on Player Collision")]
    public float knockbackTime = .678f;
    float knockbackTimer = 0f;  // neg timer from knockbackTime to 0
    float knockbackStrength = 3f;
    internal bool canMove;

    //private void Start()
    //{
    //    td = GetComponent<Enemy_TD>();
    //    rb = GetComponent<Rigidbody>();
    //}

    //private void Update()
    //{
    //    if (!aggroed && GlobalInfo.Instance.GetDistanceToPlayer(transform.position) < aggroRadius)
    //    {
    //        Debug.Log("Spikey Aggro");
    //        aggroed = true;
    //        td.canMove = false;
    //        return;
    //    }

    //    if (knockbackTimer > 0)
    //    {
    //        MoveTowardsPlayer();
    //        knockbackTimer -= Time.deltaTime;
    //    }
    //    else if (knockbackTimer < 0)
    //    {
    //        knockbackTimer = 0;
    //    }
    //}

    //private void MoveTowardsPlayer()
    //{
    //    if (!canMove)
    //        return;

    //    Debug.Log("Spikey MoveTowardsPlayer()");
    //    Vector3 dir = GlobalInfo.Instance.playerTrans.position - transform.position;
    //    rb.velocity = dir * velocitySpeed;
    //}

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.collider.CompareTag("Player"))
    //    {
    //        Knockback();
    //    }
    //}

    //private void Knockback()
    //{
    //    Vector3  knockbackDirection = -rb.velocity;
    //    knockbackDirection = knockbackDirection.normalized;
    //    knockbackTimer = knockbackTime;
    //    rb.velocity = knockbackDirection * knockbackStrength;
    //}
}
