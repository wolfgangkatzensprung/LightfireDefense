using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileHitEffect : MonoBehaviour
{
    public int damage = 1;
    public Damage.DamageType damageType;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GetComponent<Collider>().enabled = false;
            ApplyHitEffect();
            Destroy(gameObject);
        }
    }

    public virtual void ApplyHitEffect()
    {
        PlayerHealth.Instance.ApplyDamage(damage, damageType);
    }
}
