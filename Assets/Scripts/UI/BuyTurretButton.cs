using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuyTurretButton : MonoBehaviour
{
    GameObject turretPrefab;
    Transform itemHolder;
    public TextMeshProUGUI upgradeCostText;

    [Tooltip("Price of turret")]
    public int[] turretCosts = new int[16] { 100, 500, 1000, 2000, 3000, 4000, 5000, 6000, 7000, 8000, 9000, 10000, 10000, 10000, 10000, 10000};

    const int maxTurretAmount = 15;
    int currentTurretAmount = 0;


    private void Start()
    {
        turretPrefab = GlobalInfo.Instance.turretPrefab;
        itemHolder = GlobalInfo.Instance.itemHolder;

        if (!PlayerPrefs.HasKey("TurretAmount"))
            PlayerPrefs.SetInt("TurretAmount", 0);
        else
            currentTurretAmount = PlayerPrefs.GetInt("TurretAmount");
    }

    public void TryBuyTurret()
    {
        if (currentTurretAmount < maxTurretAmount && PlayerMoney.Instance.money >= turretCosts[currentTurretAmount] && EnemyWaveSpawner.Instance.isTDlevel)
        {
            PlayerMoney.Instance.RemoveMoney(turretCosts[currentTurretAmount]);

            Vector3 spawnPos = itemHolder.position + GlobalInfo.Instance.playerTrans.forward * 1.5f;
            GameObject turret = Instantiate(turretPrefab, spawnPos, Quaternion.identity);
            Turret turretComponent = turret.GetComponent<Turret>();
            currentTurretAmount += 1;
            turretComponent.turretNumber = currentTurretAmount;
            PlayerPrefs.SetInt("TurretAmount", currentTurretAmount);
            turretComponent.SaveTurretPosition();

            if (currentTurretAmount < maxTurretAmount)
                upgradeCostText.text = turretCosts[currentTurretAmount].ToString();
        }
    }
}
