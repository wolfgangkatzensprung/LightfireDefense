using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashIconDisplay : MonoBehaviour
{
    PlayerDash pd;
    Image img;

    float timer = 0f;
    float fillAmount = 0f;


    private void Start()
    {
        pd = GameObject.FindWithTag("Player").GetComponent<PlayerDash>();
        img = GetComponent<Image>();

        pd.onDash += InitiateDashCooldownIcon;
    }

    private void LateUpdate()
    {
        if (fillAmount < 1)
        {
            timer += Time.deltaTime;
            fillAmount = timer / pd.dashRepeatDelay;
            img.fillAmount = fillAmount;
        }
    }

    public void InitiateDashCooldownIcon()
    {
        timer = 0f;
        fillAmount = 0f;
    }
}
