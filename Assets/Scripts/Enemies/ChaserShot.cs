using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaserShot : EnemyProjectile
{
    public int damage = 3;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            PlayerHealth.Instance.ApplyDamage(damage, Damage.DamageType.Holo);
        }
        Destroy(gameObject);
    }
}
