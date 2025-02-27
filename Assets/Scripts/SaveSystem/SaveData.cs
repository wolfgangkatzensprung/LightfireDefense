using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    internal int[] exp;
    internal int[] levels;
    internal int money;
    internal int wave;
    internal int killCount;
    internal int[] upgrades;
    internal int lighthouseHp;  // current hp
    internal int[] keys;
    internal int orangeBuff = 0;
    internal int blueBuff = 0;

    public SaveData(int[] exp, int[] levels, int money, int wave, int killCount, int[] upgrades, int lighthouseHp, int[] keys, int orangeBuff, int blueBuff)
    {
        this.exp = exp;
        this.levels = levels;
        this.money = money;
        this.wave = wave;
        this.killCount = killCount;
        this.upgrades = upgrades;
        this.lighthouseHp = lighthouseHp;
        this.keys = keys;
        this.orangeBuff = orangeBuff;
        this.blueBuff = blueBuff;
    }
}