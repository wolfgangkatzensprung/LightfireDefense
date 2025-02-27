using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Translates GameObject to position, then disables this script
/// </summary>
public class MoveToPosition : MonoBehaviour
{
    public Vector3 targetPos = new Vector3();
    public float speed = 0.3f;
    Vector3 dir;
    float lastDistance;

    private void Start()
    {
        lastDistance = Vector3.Distance(transform.position, targetPos) + 1;
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        dir = targetPos - transform.position;
        transform.Translate(dir.normalized * speed);
        if (Vector3.Distance(transform.position, targetPos) > lastDistance)
        {
            this.enabled = false;
        }
        lastDistance = Vector3.Distance(transform.position, targetPos);
}
}
