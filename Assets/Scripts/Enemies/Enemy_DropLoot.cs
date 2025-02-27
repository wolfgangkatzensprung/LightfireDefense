using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(EnemyHealth))]
public class Enemy_DropLoot : MonoBehaviour
{
    EnemyHealth eh;

    [Tooltip("Specific drop for this mob. Drop chance 100%. Leave empty if no special drop")]
    public GameObject specialDropPrefab;

    [Tooltip("Money amount to be granted to player")]
    public int moneyAmount = 1;

    [Tooltip("Propability to drop an item")]
    public float lootDropChance = 0.01f;

    [Tooltip("Exp Gain")]
    public int exp = 10;

    Damage.DamageType expElement = Damage.DamageType.Lighthouse;   // exp type entspricht letztem received dmg type. Default ist Lighthouse Dmg da der Mob erst vom Player getroffen werden muss, um XP zu geben

    private void Start()
    {
        eh = GetComponent<EnemyHealth>();
        eh.onDeath += LootDrop;
        eh.onDamaged += LastDamageType;
    }

    private void LastDamageType(Damage.DamageType dmgType)
    {
        if (dmgType == Damage.DamageType.None)
            return;
        
        expElement = dmgType;
    }

    void LootDrop()
    {
        GrantExp();
        GrantMoney();

        DropItem();
    }

    private void DropItem()
    {
        if (specialDropPrefab != null)
        {
            Instantiate(specialDropPrefab, transform.position, Quaternion.identity);
            return;
        }
        else
            TryDropRandomItem();
    }

    private void TryDropRandomItem()
    {
        if (UnityEngine.Random.Range(0f, 1f) < lootDropChance)
            RandomLootManager.Instance.SpawnRandomItem(transform.position);
    }

    private void GrantExp()
    {
        int xpElement = (int)expElement;
        if (xpElement == (int)Damage.DamageType.Lighthouse)
            return;

        PlayerExp.Instance.AddExp(exp, xpElement);
    }

    void GrantMoney()
    {
        PlayerMoney.Instance.AddMoney(moneyAmount);
    }
}