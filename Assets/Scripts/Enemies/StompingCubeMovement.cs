using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class StompingCubeMovement : MonoBehaviour
{
    Rigidbody rb;

    // Object is only active when rendered
    bool isStomping = true;
    public Coroutine stompingRoutine;

    
    [Tooltip("Upward velocity applied to Rigidbody when jumping")]
    public float jumpVelocity = 7f;

    [Tooltip("Forward velocity applied to Rigidbody when jumping")]
    public float forwardVelocity = 3f;

    [Tooltip("Random time after a jump (min, max)")]
    public Vector2 rndDelay = new Vector2(5f, 7f);

    [Tooltip("Random rotation after a jump (min, max)")]
    public Vector2 rndRotation = new Vector2(0f, 360f);

    [Tooltip("Random resting time after rotation (min, max)")]
    public Vector2 rndRestingTime = new Vector2(0f, 3f);

    // will be triggered by SpawnCubes script
    internal bool isAggro;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        stompingRoutine = StartCoroutine(StompingRoutine());
    }

    IEnumerator StompingRoutine()
    {
        while(enabled)
        {
            if (isAggro)
            {
                ChasePlayer();
                yield return new WaitForSeconds(GetRandomRestingTime());
                ChasePlayer();
                yield return new WaitForSeconds(GetRandomDelay());
            }

            while (!isStomping)
            {
                yield return null;
            }

            Vector3 vel = Vector3.up * jumpVelocity + transform.forward * forwardVelocity;
            Vector3 euler = new Vector3(0f, Random.Range(rndRotation.x, rndRotation.y), 0f);

            rb.velocity = vel;
            yield return new WaitForSeconds(GetRandomDelay());

            rb.MoveRotation(Quaternion.Euler(euler));
            yield return new WaitForSeconds(GetRandomRestingTime());
        }
    }

    private float GetRandomDelay()
    {
        return Random.Range(rndDelay.x, rndDelay.x);
    }

    private float GetRandomRestingTime()
    {
        return Random.Range(rndRestingTime.x, rndRestingTime.y);
    }

    private void ChasePlayer()
    {
        Debug.Log("Cube chase Player");
        Vector3 dir = GlobalInfo.Instance.playerTrans.position - transform.position;
        rb.velocity = dir.normalized * 100f;
    }

    private void OnBecameVisible()
    {
        isStomping = true;
    }

    private void OnBecameInvisible()
    {
        isStomping = false;
    }
}
