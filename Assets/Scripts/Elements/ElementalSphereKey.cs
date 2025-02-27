using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalSphereKey : MonoBehaviour
{
    public ShowOrbTextOnCollect sotoc;
    public SphereKeys.KeyType keyType;

    [Tooltip("Exp granted on pickup")]
    public int exp = 500;

    private void Start()
    {
        if (SphereKeys.HasKey(keyType))
        {
            Destroy(gameObject);
            return;
        }

        if (PlayerInputManager.Instance != null)
            PlayerInputManager.Instance.onItemPickup += OnPickup;
    }

    private void OnPickup()
    {
        if (gameObject != null && GlobalInfo.Instance.heldItem.Equals(gameObject))
        {
            EarnKey();
        }
    }

    private void EarnKey()
    {
        SphereKeys.AddKey((int)keyType);

        sotoc.ShowOrbCollectedUI();
        ElementalKeyExpAndEffects();

        SaveSystem.SaveGame();
        Destroy(gameObject);
    }

    private void ElementalKeyExpAndEffects()
    {
        int xp = 0;
        Damage.DamageType xpType = Damage.DamageType.None;
        switch(keyType)
        {
            case SphereKeys.KeyType.Earth:
                xp = this.exp;
                xpType = Damage.DamageType.Earth;
                break;
            case SphereKeys.KeyType.Fire:
                xp = this.exp;
                xpType = Damage.DamageType.Fire;
                PlayerShooting.Instance.currentBonusDmg = PlayerShooting.maxBonusDmg;
                break;
            case SphereKeys.KeyType.Water:
                xp = this.exp;
                xpType = Damage.DamageType.Water;
                break;
            case SphereKeys.KeyType.Air:
                xp = this.exp;
                xpType = Damage.DamageType.Air;
                break;
        }
        PlayerExp.Instance.AddExp(xp, (int)xpType);
    }

    private void OnDisable()
    {
        if (PlayerInputManager.Instance != null)
            PlayerInputManager.Instance.onItemPickup -= OnPickup;
    }
}