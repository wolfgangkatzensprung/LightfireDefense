using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Trap))]
public class TrapFusion : MonoBehaviour
{
    public GameObject turretPrefab;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent(out Trap trapComponent))
        {
            if (trapComponent.trapType == GetComponent<Trap>().trapType)
            {
                DoFusion(collision);
            }
        }
    }

    private void DoFusion(Collision col)
    {
        if (col.gameObject == null)
            Destroy(col.gameObject);

        Instantiate(turretPrefab, transform.position, Quaternion.identity);
    }
}
