using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroDoor : MonoBehaviour, IDamageable
{
    public GameObject[] lamps = new GameObject[4];
    [Tooltip("Element Materials in following order: Air, Earth, Fire, Water")]
    public Material[] mats = new Material[4];

    bool[] doorOpening = new bool[4];

    public delegate void DamagedDelegate(Damage.DamageType dmgType);
    public DamagedDelegate onDamaged;

    public delegate void DeathDelegate();
    public DeathDelegate onDeath;

    public void ApplyDamage(int damage, Damage.DamageType dmgType)
    {
        int i = -1;
        switch (dmgType)
        {
            default:
                i = -1;
                break;
            case Damage.DamageType.Air:
                i = 0;
                break;
            case Damage.DamageType.Earth:
                i = 1;
                break;
            case Damage.DamageType.Fire:
                i = 2;
                break;
            case Damage.DamageType.Water:
                i = 3;
                break;
        }
        if (i >= 0)
        {
            lamps[i].GetComponent<Renderer>().material = mats[i];
            lamps[i].transform.GetChild(0).gameObject.SetActive(true);

            doorOpening[i] = true;
            if (doorOpening[0] && doorOpening[1] && doorOpening[2] && doorOpening[3])
            {
                Die();
            }

            onDamaged?.Invoke(dmgType);
        }
    }

    public void Die()
    {
        onDeath?.Invoke();
        Destroy(gameObject);
    }

    public bool GetIsAlive()
    {
        return true;
    }

    public void TryApplyDamage(int damage, Damage.DamageType damageType)
    {
        ApplyDamage(damage, damageType);
    }
}
