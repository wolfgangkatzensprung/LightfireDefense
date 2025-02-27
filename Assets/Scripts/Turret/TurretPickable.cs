using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretPickable : MonoBehaviour, IEquipable, IInteractable
{
    Rigidbody rb;
    Collider col;
    Turret tur;

    Transform itemHolder;


    private void Start()
    {
        itemHolder = GlobalInfo.Instance.itemHolder;
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        tur = GetComponent<Turret>();
    }

    public void Equip()
    {
        foreach(Transform children in transform)
        {
            if (TryGetComponent(out Collider collider))
            {
                collider.enabled = false;
            }
        }

        rb.velocity = Vector3.zero;
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        GlobalInfo.Instance.SetHeldItem(gameObject);
        transform.position = itemHolder.transform.position;
        transform.parent = itemHolder;
    }

    public void Unequip()
    {
        foreach (Transform children in transform)
        {
            if (TryGetComponent(out Collider collider))
            {
                collider.enabled = true;
            }
        }

        transform.parent = null;
        GlobalInfo.Instance.SetHeldItem(null);
        rb.constraints = RigidbodyConstraints.None;
        rb.useGravity = true;

        tur.SaveTurretPosition();
        GameController.MoveObjectToCurrentLevelScene(gameObject);
    }

    public void Interact()
    {
        SoundManager.Instance.PlaySoundAt(SoundManager.Sound.Interact, transform.position);

        tur.SwitchTargetMode();
    }
}
