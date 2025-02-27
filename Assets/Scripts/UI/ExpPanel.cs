using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExpPanel : MonoBehaviour
{
    // Reihenfolge: Erde, Feuer, Wasser, Luft
    public Image[] elementalBars;

    public TextMeshProUGUI[] levelTexts;
    //public TextMeshProUGUI playerLevelText;

    const int earth = (int)PlayerExp.ExpType.Earth;
    const int fire = (int)PlayerExp.ExpType.Fire;
    const int water = (int)PlayerExp.ExpType.Water;
    const int air = (int)PlayerExp.ExpType.Air;

    private void OnEnable()
    {
        UpgradeExpBars();
    }

    private void UpgradeExpBars()
    {
        //Debug.Log("UpdateExpBars");

        elementalBars[0].fillAmount = (float)PlayerExp.Instance.exp[earth] / PlayerExp.Instance.levelUpCost[earth];
        //Debug.Log($"EarthExp fill is {(float)PlayerExp.Instance.exp[earth]} / {PlayerExp.Instance.levelUpCost[earth]}");
        elementalBars[1].fillAmount = (float)PlayerExp.Instance.exp[fire] / PlayerExp.Instance.levelUpCost[fire];
        elementalBars[2].fillAmount = (float)PlayerExp.Instance.exp[water] / PlayerExp.Instance.levelUpCost[water];
        elementalBars[3].fillAmount = (float)PlayerExp.Instance.exp[air] / PlayerExp.Instance.levelUpCost[air];

        levelTexts[0].text = PlayerExp.Instance.level[earth].ToString();
        levelTexts[1].text = PlayerExp.Instance.level[fire].ToString();
        levelTexts[2].text = PlayerExp.Instance.level[water].ToString();
        levelTexts[3].text = PlayerExp.Instance.level[air].ToString();
        //playerLevelText.text = $"Level {PlayerExp.Instance.playerLevel.ToString()}";
    }
}