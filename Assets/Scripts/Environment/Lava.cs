using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    public int damage = 3;

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log($"{damage} Lava Dmg");
            PlayerHealth.Instance.ApplyDamage(damage, Damage.DamageType.Fire);
        }
    }
}