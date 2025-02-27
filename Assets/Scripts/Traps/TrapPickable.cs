using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrapPickable : MonoBehaviour, IEquipable
{
    Rigidbody rb;
    Transform itemHolder;
    Trap trap;

    private void Start()
    {
        itemHolder = GlobalInfo.Instance.itemHolder;
        rb = GetComponent<Rigidbody>();
        trap = GetComponent<Trap>();
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

    public void Unequip()
    {
        transform.parent = null;

        GlobalInfo.Instance.SetHeldItem(null);
        rb.constraints = RigidbodyConstraints.None;
        rb.useGravity = true;

        if (TryGetComponent(out Collider col))
            col.enabled = true;

        TrapManager.Instance.SaveTrap(trap);
        GameController.MoveObjectToCurrentLevelScene(gameObject);
    }
}
