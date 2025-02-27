using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCatcher : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out ItemPickable ip) || collision.gameObject.TryGetComponent(out IEquipable equipable))
        {
            collision.transform.position = collision.transform.position + Vector3.up * 200f;
        }
    }
}