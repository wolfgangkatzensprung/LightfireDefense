using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    public Damage.DamageType damageType;
    internal Rigidbody rb;

    public float speed = 100f;
    public float selfDestructionTime = 7f;

    private void Start()
    {
        if (damageType == Damage.DamageType.Water)
        {
            SoundManager.Instance.PlaySoundAt(SoundManager.Sound.WaterProjectile, transform.position);
        }
        Destroy(gameObject, selfDestructionTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        SoundManager.Instance.PlaySoundAt(SoundManager.Sound.ProjectileHit, collision.GetContact(0).point);

        if (collision.transform.TryGetComponent(out IDamageable eh))
        {
            eh.TryApplyDamage(PlayerShooting.Instance.damageMultiplier * PlayerExp.Instance.level[(int)damageType] + PlayerShooting.Instance.currentBonusDmg, damageType);   // dmg is equal to the level of corresponding element * damageMultiplier + bonusDmg
        }

        if (transform.childCount > 0)
            Destroy(transform.GetChild(0).gameObject, selfDestructionTime * .5f);
        transform.DetachChildren();

        Destroy(gameObject);
    }
}
