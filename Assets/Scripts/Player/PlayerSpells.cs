using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpells : Singleton<PlayerSpells>
{
    [Header("References")]
    Ray ray;
    RaycastHit hit;
    PlayerInputManager inputInstance;
    PlayerMana manaInstance;

    [Tooltip("Alle Spells die es gibt. Reihenfolge: Erde, Feuer, Wasser, Luft")]
    public GameObject[] spellPrefabs = new GameObject[1];

    // Reihenfolge entspricht spellPrefabs
    int spellIndex = 0;

    //Dictionary<Damage.DamageType, int> currentSpellLevels = new Dictionary<Damage.DamageType, int>();

    [Header("Settings")]
    [Tooltip("Maximum spell casting distance from Player")]
    public float maxSpellRange = 100f;
    [Tooltip("Layers that are hit by SpellField Raycast")]
    public LayerMask raycastLayers;

    private void Start()
    {
        inputInstance = PlayerInputManager.Instance;
        manaInstance = PlayerMana.Instance;

        inputInstance.onSpellCast += TrySpellCast;
        //PlayerExp.Instance.onLevelUp += UpgradeSpellByDamageType;
    }

    void TrySpellCast()
    {
        if (UIManager.Instance.inMenu)
            return;

        if (manaInstance.currentMana < manaInstance.spellCost)
        {
            SoundManager.Instance.PlayNonspacialSound(SoundManager.Sound.Error);
            return;
        }

        Damage.DamageType currentDamageType = GetDamageTypeFromSpellIndex(spellIndex);

        Debug.Log("TrySpellCast() with " + currentDamageType.ToString());

        if (PlayerExp.Instance.level[(int)currentDamageType] > 2)
        {
            SpellCast();
        }
        else if (PlayerExp.Instance.level[(int)currentDamageType] != 0)
        {
            SoundManager.Instance.PlayNonspacialSound(SoundManager.Sound.Error);
        }

        //if (currentSpellLevels.ContainsKey(currentDamageType))
        //{
        //    Debug.Log($"currentSpellLevels contains {currentDamageType}");
        //    if (currentSpellLevels[currentDamageType] > 1)
        //    {
        //        SpellCast();
        //    }
        //}
    }

    private Damage.DamageType GetDamageTypeFromSpellIndex(int spellIndex)
    {
        switch (spellIndex)
        {
            case 0:
                return Damage.DamageType.Earth;
            case 1:
                return Damage.DamageType.Fire;
            case 2:
                return Damage.DamageType.Water;
            case 3:
                return Damage.DamageType.Air;
        }

        return Damage.DamageType.None;
    }

    void SpellCast()
    {
        manaInstance.UseMana_SpellCast();

        ray = MainCamRaycast.Instance.GetRay();
        if (Physics.Raycast(ray, out hit, maxSpellRange, raycastLayers))
        {
            //Debug.Log($"SpellCast: {spellPrefabs[spellIndex].name}");
            Instantiate(spellPrefabs[spellIndex], hit.point, Quaternion.identity);
        }
        else
        {
            Vector3 firePointPos = GlobalInfo.Instance.firePoint.transform.position;
            Vector3 direction = ray.direction.normalized * maxSpellRange;
            Instantiate(spellPrefabs[spellIndex], firePointPos + direction, Quaternion.identity);
        }
    }

    public void SwitchSpell(int spellIndex)
    {
        this.spellIndex = spellIndex;
    }

    //private void UpgradeSpellByDamageType(Damage.DamageType dmgType)
    //{
    //    Debug.Log($"UpgradeSpellByDamageType({dmgType.ToString()})");

    //    if (currentSpellLevels.ContainsKey(dmgType))
    //    {
    //        currentSpellLevels[dmgType] += 1;
    //        Debug.Log($"Spell of type {dmgType} properly upgraded to {currentSpellLevels[dmgType]}");
    //    }
    //}

    //internal void LoadSpells(int[] spellLevels)
    //{
    //    for (int i = 0; i < spellLevels.Length; i++)
    //    {
    //        currentSpellLevels.Add((Damage.DamageType)i, spellLevels[i]);
    //    }
    //}
    
    private void OnDisable()
    {
#if UNITY_EDITOR
        return;
#endif

        inputInstance.onSpellCast -= SpellCast;
        inputInstance.onMouseScroll -= SwitchSpell;
    }
}
