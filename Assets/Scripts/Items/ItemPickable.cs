using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ItemPickable : MonoBehaviour, IEquipable
{
    internal Rigidbody rb;
    Transform itemHolder;

    private void Start()
    {
        if (GlobalInfo.Instance != null)
            itemHolder = GlobalInfo.Instance.itemHolder;
        rb = GetComponent<Rigidbody>();
    }

    public void Equip()
    {
        if (TryGetComponent(out Collider col))
            col.enabled = false;

        rb.velocity = Vector3.zero;
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;

        GlobalInfo.Instance.SetHeldItem(gameObject);
        transform.position = itemHolder.transform.position;
        transform.parent = itemHolder;
    }

    public virtual void Unequip()
    {
        transform.parent = null;
        GlobalInfo.Instance.SetHeldItem(null);
        rb.constraints = RigidbodyConstraints.None;
        rb.useGravity = true;

        if (TryGetComponent(out Collider col))
            col.enabled = true;
    }
}
