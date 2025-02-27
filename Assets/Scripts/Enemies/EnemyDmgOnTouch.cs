using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDmgOnTouch : MonoBehaviour
{
    [Tooltip("Damage dealt to Player")]
    public int damage = 2;

    [Tooltip("Damage Type")]
    public Damage.DamageType dmgType;

    public delegate void DamageDelegate();  // when Enemy damages player
    public DamageDelegate onDamage;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            PlayerHealth.Instance.ApplyDamage(damage, dmgType);
            onDamage?.Invoke();
        }
    }
}
