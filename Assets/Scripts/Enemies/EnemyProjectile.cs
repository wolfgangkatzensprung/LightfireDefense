using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyProjectile : MonoBehaviour
{
    Transform playerTrans;
    Rigidbody rb;

    Vector3 direction;

    public float speed = 100f;

    [Tooltip("When activated, Projectile will chase Player")]
    public bool homingMissile;
    float homingTimer = 0f;
    [Tooltip("Maximum acceleration will be reached after this amount of seconds")]
    public float maxHomingTimer = 10f;

    [Tooltip("Maximum lifetime in seconds")]
    public float maxLifeTime = 10f;

    private void Start()
    {
        playerTrans = GlobalInfo.Instance.playerTrans;
        rb = GetComponent<Rigidbody>();
        direction = (playerTrans.position + Vector3.up * 2 - transform.position).normalized;
        rb.AddForce(direction * speed, ForceMode.Impulse);
    }

    private void Update()
    {
        if (homingMissile)
        {
            if (homingTimer < maxHomingTimer)
                homingTimer += Time.deltaTime;

            direction = playerTrans.position + Vector3.up * 2 - transform.position;
            rb.AddForce(direction.normalized * speed * homingTimer, ForceMode.Acceleration);
        }
    }
}