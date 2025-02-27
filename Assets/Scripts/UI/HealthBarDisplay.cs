using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBarDisplay : MonoBehaviour
{
    PlayerHealth pxp;
    Image healthBar;

    private void Start()
    {
        healthBar = GetComponent<Image>();
        pxp = GlobalInfo.Instance.playerTrans.GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        DisplayHealth();
    }

    private void DisplayHealth()
    {
        healthBar.fillAmount = (float)pxp.GetHealth() / (float)pxp.maxHealth;
        //Debug.Log($"ph.GetHealth() = {pxp.GetHealth()} / ph.maxHealth = {pxp.maxHealth}");
    }
}
