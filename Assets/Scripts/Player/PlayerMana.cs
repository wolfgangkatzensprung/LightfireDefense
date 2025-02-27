using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMana : Singleton<PlayerMana>
{
    [Header("Settings")]
    internal float maxMana = 25;
    internal float currentMana;
    internal float deltaMana = .1f;   // Mana pro Time.deltaTime Tick
    float startDeltaMana = .1f;
    internal int blueBuffLevel = 0;   // mana reg bonus delta

    bool manaActive = true;

    [Tooltip("Mana Cost for single Attack")]
    public float attackManaCost = 1f;
    [Tooltip("Mana Cost for single SpellCast")]
    public float spellCost = 15f;

    public delegate void ManaUsedDelegate();
    public ManaUsedDelegate onManaUsed;
    public delegate void BlueBuffUpgradeDelegate();
    public BlueBuffUpgradeDelegate onBlueBuffUpgrade;

    private void Start()
    {
        currentMana = maxMana;
        StartCoroutine(RefillMana());
    }

    internal void UseMana_Attack() => UseMana(attackManaCost);
    internal void UseMana_SpellCast() => UseMana(spellCost);

    internal void LoadBlueBuff()
    {
        SaveData saveData = SaveSystem.LoadGame();
        deltaMana = startDeltaMana + saveData.blueBuff * .01f;
    }
    internal void UpgradeBlueBuff()
    {
        blueBuffLevel += 1;
        deltaMana = startDeltaMana + blueBuffLevel * .01f;
        SaveSystem.SaveGame();

        onBlueBuffUpgrade?.Invoke();
    }
    internal void ResetBlueBuff()
    {
        blueBuffLevel = 0;
        deltaMana = startDeltaMana;
    }

    internal void UseMana(float manaUsed)
    {
        currentMana = Mathf.Max(0, currentMana - manaUsed);
        onManaUsed?.Invoke();
    }

    IEnumerator RefillMana()
    {
        while (manaActive)
        {
            currentMana = Mathf.Min(maxMana, currentMana + deltaMana);
            UpdateManaUI();
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    void UpdateManaUI()
    {
        if (UIManager.Instance == null)
            return;

        string manaText = ((int)currentMana).ToString();
        UIManager.Instance.SetManaText(manaText);
    }

    internal void SetMana(int mana)
    {
        maxMana = mana;
        UpdateManaUI();
    }
}