using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoney : Singleton<PlayerMoney>
{
    public int money = 0;
    const int maxMoney = 999999999;

    public delegate void GetMoneyDelegate();
    public GetMoneyDelegate onGetMoney;

    public delegate void LoseMoneyDelegate();
    public LoseMoneyDelegate onLoseMoney;

    private void Start()
    {
        onGetMoney += TryUpdateMoneyTextUI;
        onLoseMoney += TryUpdateMoneyTextUI;
    }

    internal void SetMoney(int money)
    {
        this.money = money;
        TryUpdateMoneyTextUI();
    }

    public void TryUpdateMoneyTextUI()
    {
        if (UIManager.Instance == null)
            return;

        string moneyText = ((int)money).ToString();
        UIManager.Instance.SetMoneyText(moneyText);
    }

    public void AddMoney(int moreMoney)
    {
        Debug.Log($"Added {moreMoney} Money");
        money = Mathf.Min(money + moreMoney, maxMoney);

        onGetMoney?.Invoke();
    }

    public void RemoveMoney(int money)
    {
        this.money = Mathf.Max(0, this.money - money);

        onLoseMoney?.Invoke();
    }
}
