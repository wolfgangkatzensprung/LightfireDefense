using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerBuild : Singleton<PlayerBuild>
{
    public GameObject turretPreviewObject;

    internal bool isBuilding;

    GameObject turretPrefab;
    //public TextMeshProUGUI upgradeCostText;

    [Tooltip("Price of turret")]
    public int[] turretCosts = new int[15] { 100, 500, 1000, 2000, 3000, 4000, 5000, 6000, 7000, 8000, 9000, 10000, 10000, 10000, 10000 };

    const int maxTurretAmount = 15;
    int currentTurretAmount = 0;
    string maxText = "Maximum reached.";

    internal delegate void FirstTurretPlacedDelegate();
    internal FirstTurretPlacedDelegate onFirstTurretPlaced;

    void Start()
    {
        turretPrefab = GlobalInfo.Instance.turretPrefab;

        if (!PlayerPrefs.HasKey("TurretAmount"))
            PlayerPrefs.SetInt("TurretAmount", 0);
        else
            currentTurretAmount = PlayerPrefs.GetInt("TurretAmount");

        PlayerInputManager.Instance.onBuild += TryStartTurretBuilding;
        PlayerInputManager.Instance.onCancelBuild += CancelTurretBuilding;
        PlayerInputManager.Instance.onAttack += FinishBuild;
        SceneLoading.Instance.onSceneLoadedAsync += TrySetTurretUI;

    }

    private void TrySetTurretUI(string sceneName)
    {
        if (sceneName == "TD Level")
        {
            SetTurretAmountText();
            SetTurretCostText();
        }
    }
    private void SetTurretAmountText()
    {
        UIManager.Instance.turretAmountText.text = $"{currentTurretAmount} / {maxTurretAmount}";
    }
    private void SetTurretCostText()
    {
        if (currentTurretAmount < maxTurretAmount)
            UIManager.Instance.turretUpgradeCostText.text = turretCosts[currentTurretAmount].ToString();
        else
            UIManager.Instance.turretUpgradeCostText.text = maxText;
    }

    private void TryStartTurretBuilding()
    {
        if (isBuilding || !(currentTurretAmount < maxTurretAmount) || !EnemyWaveSpawner.Instance.isTDlevel)
        {
            CancelTurretBuilding();
            return;
        }

        if (PlayerMoney.Instance.money < turretCosts[currentTurretAmount])
        {
            UIManager.Instance.PlayNotEnoughMoneyAnim();
            SoundManager.Instance.PlayNonspacialSound(SoundManager.Sound.Error);
            return;
        }

        PlayerShooting.Instance.canShoot = false;
        turretPreviewObject.SetActive(true);
        isBuilding = true;
    }

    private void CancelTurretBuilding()
    {
        isBuilding = false;
        turretPreviewObject.SetActive(false);
        PlayerShooting.Instance.canShoot = true;
    }

    private void FinishBuild()
    {
        if (isBuilding)
        {
            TryBuyTurret();
        }

        CancelTurretBuilding();
    }

    public void TryBuyTurret()
    {
        if (currentTurretAmount < maxTurretAmount && PlayerMoney.Instance.money >= turretCosts[currentTurretAmount] && EnemyWaveSpawner.Instance.isTDlevel)
        {
            PlayerMoney.Instance.RemoveMoney(turretCosts[currentTurretAmount]);

            GameObject turret = Instantiate(turretPrefab, turretPreviewObject.transform.position, turretPreviewObject.transform.rotation);
            Turret turretComponent = turret.GetComponent<Turret>();
            currentTurretAmount += 1;
            turretComponent.turretNumber = currentTurretAmount;
            PlayerPrefs.SetInt("TurretAmount", currentTurretAmount);
            turretComponent.SaveTurretPosition();

            Debug.Log($"Turret placed. currentTurretAmount is {currentTurretAmount}");
            if (currentTurretAmount < 2)
            {
                onFirstTurretPlaced?.Invoke();
            }

            if (currentTurretAmount < maxTurretAmount)
                SetTurretCostText();
            else
                UIManager.Instance.turretUpgradeCostText.text = maxText;

            SaveSystem.SaveGame();
        }

        SetTurretAmountText();
    }

    internal void ResetBuildingUI() // turret buy texts reset to defaults
    {
        currentTurretAmount = 0;
        if (UIManager.Instance != null)
        {
            UIManager.Instance.turretAmountText.text = currentTurretAmount.ToString();
            SetTurretCostText();
        }
    }
}