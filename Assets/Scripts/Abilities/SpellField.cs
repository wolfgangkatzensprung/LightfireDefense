using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellField : Spell
{
    public enum SpellType
    {
        Water,
        Fire,
        Air,
        Earth,
        Time,
        Arcane
    }
    public SpellType spellType;

    [Tooltip("Layers that this spell will interact with")]
    public LayerMask layersToHit;

    private void Start()
    {
        CastSpell();
    }

    public override void CastSpell()
    {
        foreach (GameObject spellVFX in spellPrefabsVFX)
            Instantiate(spellVFX, transform.position, Quaternion.identity);

        StartCoroutine(SpellFieldRoutine());

        base.CastSpell();
    }

    IEnumerator SpellFieldRoutine()
    {
        SoundManager.Instance.PlaySoundAt(SoundManager.Sound.SpellField, transform.position);

        while (true)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, damageRadius, layersToHit, QueryTriggerInteraction.Ignore);

            if (hits.Length > 0)
            {
                for (int i = 0; i < hits.Length; i++)
                {
                    Debug.Log($"{spellType} Spell hit {hits[i].name}");
                    DealSpellFieldDamage(hits, i);
                    TryApplySpellEffect(hits, i);
                }
            }

            yield return new WaitForSeconds(tickRate);

            if (currentSpellTime > spellLength)
            {
                EndSpell();
            }
        }
    }

    private void TryApplySpellEffect(Collider[] hits, int i)
    {
        Debug.Log("TryApplySpellEffect");
        if (hits[i].transform.TryGetComponent(out ISpellEffectListener spellEffector))
        {
            Debug.Log($"Apply SpellEffect ({spellType}) to {hits[i].gameObject.name}");
            spellEffector.ApplySpellEffect(spellType);
        }
    }

    private void DealSpellFieldDamage(Collider[] hits, int i)
    {
        Debug.Log($"Collider {hits[i]} hit by SpellDamageField()");

        if (hits[i].TryGetComponent(out EnemyHealth enemyHealthComponent))
        {
            damagePerTick = PlayerExp.Instance.level[((int)spellType + 1)];
            Debug.Log($"Applying damage to {hits[i].gameObject.name}");
            Damage.DamageType damageType = (Damage.DamageType)((int)spellType + 1);
            enemyHealthComponent.TryApplyDamage(damagePerTick, damageType);
        }
    }

    public override void EndSpell()
    {
        base.EndSpell();    // = destroy
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, .5f);
        Gizmos.DrawSphere(transform.position, damageRadius);
    }
}
