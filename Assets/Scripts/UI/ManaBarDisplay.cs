using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ManaBarDisplay : MonoBehaviour
{
    PlayerMana pMana;
    public Image manaBar;
    public Image manaBarBackground;
    public TextMeshProUGUI manaText;

    Color visibleColor = new Color(1f, 1f, 1f, 1f);
    Color invisibleColor = new Color(1f, 1f, 1f, 0f);

    bool visible;

    float fadeInTime = .1f;
    float fadeInTimer = 0f;
    float fadeOutTime = 1f;
    float fadeOutTimer = 0f;
    bool fadeIn;
    bool fadeOut;

    private void Start()
    {
        pMana = PlayerMana.Instance;
        pMana.onManaUsed += TryStartFadeIn;
    }

    private void LateUpdate()
    {
        if (visible)
        {
            DisplayMana();
        }
        if (fadeIn)
        {
            FadeIn();
        }
        else if (fadeOut)
        {
            FadeOut();
        }
    }

    private void DisplayMana()
    {
        manaBar.fillAmount = pMana.currentMana / pMana.maxMana;
        if (pMana.currentMana >= pMana.maxMana && !fadeIn && !fadeOut)
        {
            StartFadeOut();
        }
    }

    private void StartFadeOut()
    {
        CancelFadeIn();

        visible = true;
        fadeOut = true;
    }

    private void FadeOut()
    {
        Color lerpColor = Color.Lerp(visibleColor, invisibleColor, fadeOutTimer);
        visible = true;

        manaBar.color = lerpColor;
        manaText.color = lerpColor;
        manaBarBackground.color = lerpColor;

        fadeOutTimer += Time.deltaTime;
        if (fadeOutTimer > 1f)
        {
            CancelFadeOut();
            visible = false;
        }
    }

    private void CancelFadeOut()
    {
        fadeOut = false;
        fadeOutTimer = 0f;
    }

    void TryStartFadeIn()
    {
        CancelFadeOut();

        if (fadeIn)
            return;

        StartFadeIn();
    }

    void StartFadeIn()
    {
        visible = true;
        fadeIn = true;
    }

    private void FadeIn()
    {
        Color lerpColor = Color.Lerp(invisibleColor, visibleColor, fadeInTimer);
        visible = true;

        manaBar.color = lerpColor;
        manaText.color = lerpColor;
        manaBarBackground.color = lerpColor;

        fadeInTimer += Time.deltaTime;
        if (fadeInTimer > 1f)
        {
            CancelFadeIn();
        }
    }

    private void CancelFadeIn()
    {
        fadeIn = false;
    }
}
