using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spell : MonoBehaviour, ICastable
{
    [Header("References")]

    [Tooltip("Spell VFX Graphs")]
    public GameObject[] spellPrefabsVFX = new GameObject[1];

    [Header("Settings")]

    [Tooltip("Damage range")]
    public float damageRadius = 5f;

    [Tooltip("Damage that is dealt each tick to enemies in range")]
    public int damagePerTick = 1;

    [Tooltip("Seconds till next tick - Default is 0.1f (= every 10th of a second)")]
    public float tickRate = .1f;

    [Tooltip("Spell Length in seconds")]
    public float spellLength = 5f;
    internal float currentSpellTime = 0f;

    private void Update()
    {
        currentSpellTime += Time.deltaTime;
    }

    public virtual void CastSpell()
    {
        // override this
    }

    public virtual void EndSpell()
    {
        // override this
        Destroy(gameObject);
    }
}
