using System;
using UnityEngine;

public class PlayerExp : Singleton<PlayerExp>
{
    public enum ExpType // sehr aehnlich wie Damage.DamageType und kann daher ineinander umgewandelt werden per (int)cast. 
    {
        None,
        Water,
        Fire,
        Air,
        Earth,
        Time,
        Arcane,
        Holo,
        Envoy,
        Shadow,
        Lighthouse
    }
    internal ExpType lastExpType;

    const int expTypesAmount = 9;
    const int maxExp = 999999999;
    const int maxLevel = 99;
    const int maxPlayerLevel = 999;

    // Experience gained since levelup for each Element
    internal int[] exp = new int[expTypesAmount];

    // Level
    internal int[] level { get; set; } = new int[expTypesAmount];   // each Element
    internal int[] levelUpCost = new int[expTypesAmount];           // cost for each Element
    [SerializeField]
    internal int playerLevel = 4;                                  // Gesamtlevel ergibt sich aus einzelnen ElementarLevels

    public delegate void GetExpDelegate();
    public GetExpDelegate onGetExp;

    public delegate void LoseExpDelegate();
    public LoseExpDelegate onLoseExp;

    public delegate void LevelUpDelegate(Damage.DamageType dmgType);
    public LevelUpDelegate onLevelUp;

    private void Start()
    {
        InitializeLevelsIfRequired();

        GameController.Instance.onStartNewGame += InitializeLevels;
    }

    private void InitializeLevelsIfRequired()
    {
        if (level[0] != 0)
            return;

        InitializeLevels();
    }

    private void InitializeLevels()
    {
        Debug.Log("InitializeLevels");

        playerLevel = 4;

        for (int i = 0; i < expTypesAmount; i++)
        {
            level[i] = 1;
            levelUpCost[i] = 10;
            exp[i] = 0;
            levelUpCost[i] = 100;
        }
    }

    public void AddExp(int expGained, int xpIndex)
    {
        Debug.Log($"Add {expGained} exp at {(Damage.DamageType)xpIndex}");

        lastExpType = (ExpType)xpIndex;

        if (exp[xpIndex] + expGained > levelUpCost[xpIndex])
        {
            LevelUp(xpIndex);
            return;
        }
        else
        {
            exp[xpIndex] = Mathf.Min(exp[xpIndex] + expGained, maxExp);
        }

        onGetExp?.Invoke();
    }

    internal void LevelUp(int xpIndex)
    {
        level[xpIndex] = Math.Min(level[xpIndex] + 1 , maxLevel);
        playerLevel = Math.Min(playerLevel + 1, maxPlayerLevel);
        levelUpCost[xpIndex] = Mathf.Min(200 * level[xpIndex], maxExp);
        exp[xpIndex] = 0;

        UpgradeManager.Instance.Upgrade(UpgradeManager.UpgradeType.PlayerHp);
        UpgradeManager.Instance.Upgrade(UpgradeManager.UpgradeType.PlayerMana);

        onLevelUp?.Invoke(GetDamageTypeFromExpTypeIndex(xpIndex));
    }

    private Damage.DamageType GetDamageTypeFromExpTypeIndex(int xpIndex)
    {
        switch (xpIndex)
        {
            case 1:
                return Damage.DamageType.Water;
            case 2:
                return Damage.DamageType.Fire;
            case 3:
                return Damage.DamageType.Air;
            case 4:
                return Damage.DamageType.Earth;
        }

        return Damage.DamageType.None;
    }

    public void RemoveExp(int xp, int xpIndex)
    {
        this.exp[xpIndex] = Mathf.Max(0, this.exp[xpIndex] - xp);

        onLoseExp?.Invoke();
    }

    internal void SetExp(int[] exp)
    {
        this.exp = exp;
    }

    internal void SetLevels(int[] levels)
    {
        this.level = levels;
        int levelsLength = 4;   // amount of different elements

        playerLevel = 0;
        for (int i = 0; i < levelsLength; i++)
        {
            Debug.Log($"level {i}: {levels[i]}");
            playerLevel += levels[i];
        }

        SetLevelUpThreshold(levels);
        //PlayerSpells.Instance.LoadSpells(levels);
    }

    private void SetLevelUpThreshold(int[] level)
    {
        for (int i = 0; i < level.Length; i++)
        {
            levelUpCost[i] = Mathf.Min(100 * level[i], maxExp);
            Debug.Log($"levelUpCost[{i}]: {100 * level[i]} Exp");
        }
    }

    internal Color GetColorFromLastExp()
    {
        Color col = Color.white;

        switch (lastExpType)
        {
            case ExpType.Air:
                col = new Color(1, 0.92f, 0.016f, 0);
                break;
            case ExpType.Earth:
                col = new Color(0, 1, 0, 0);
                break;
            case ExpType.Fire:
                col = new Color(1, 0, 0, 0);
                break;
            case ExpType.Water:
                col = new Color(0, 0, 1, 0);
                break;
        }

        return col;
    }


    private void OnDisable()
    {
        if (GameController.Instance != null)
            GameController.Instance.onStartNewGame -= InitializeLevels;
    }
}