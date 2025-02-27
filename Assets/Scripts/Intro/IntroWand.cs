using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroWand : MonoBehaviour, IEquipable
{
    private void Start()
    {
        PlayerInputManager.Instance.onItemPickup += OnPickup;
    }

    public void Equip()
    {
        GlobalInfo.Instance.magicWand.SetActive(true);
        GlobalInfo.Instance.playerTrans.GetComponent<PlayerShooting>().wandEquipped = true;

        Destroy(gameObject);
    }

    public void Unequip()
    {
    }

    public void OnPickup()
    {
        if (GlobalInfo.Instance.heldItem != null && GlobalInfo.Instance.heldItem.Equals(gameObject))
        {
            Destroy(gameObject);
        }
    }

    private void OnDisable()
    {
#if UNITY_EDITOR
        return;
#endif
        PlayerInputManager.Instance.onItemPickup -= OnPickup;
    }
}
