using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using ECM.Components;
using System;

public class UIManager : Singleton<UIManager>
{
    [Header("TMPro References")]
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI manaText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI waveIndexText;
    public TextMeshProUGUI killCountText;
    public TextMeshProUGUI turretUpgradeCostText;
    public TextMeshProUGUI turretAmountText;

    [Header("GameObject References")]
    [Tooltip("Main UI GameObject")] public GameObject UI;
    public GameObject startNextWave;
    public GameObject UIPanel;
    public GameObject escapePanel;
    public GameObject bossHpBar;
    public GameObject itemPickupText;
    public GameObject interactText;
    public GameObject introPaper;
    public GameObject orbCollectedPanel;
    public GameObject youLost;
    public GameObject youWin;
    public GameObject turretBuildPanel;
    public Tooltip tooltip;
    [Tooltip("Name des anvisierten Objekts")]
    public TextMeshProUGUI itemNameText;

    [Header("Animation")]
    public Animator healedAnim;
    public Animator notEnoughMoneyAnim;

    [Header("LoadingScreen")]
    public GameObject loadingScreen;
    public Slider loadingSlider;

    MouseLook mouseLook;
    BossHpBarDisplay bossHpBarDisplayComponent;

    [SerializeField]
    internal bool inMenu;
    [SerializeField]
    internal bool inIntro;
    [SerializeField]
    internal bool inLostScreen;
    [SerializeField]
    internal bool isUpgradeMenu;    // true: UpgradeMenu, false: EscapeMenu

    public override void Awaken()
    {
        Debug.Log("UIManager Awaken");

        if (!UI.activeSelf)
        {
            Debug.Log("Activate UI");
            UI.SetActive(true);
        }
        else
            Debug.Log("Activating UI failed since it is already active.");
    }

    private void Start()
    {
        mouseLook = GlobalInfo.Instance.mouseLook;
        bossHpBarDisplayComponent = bossHpBar.GetComponentInChildren<BossHpBarDisplay>();
        LighthouseManager.Instance.Initialize();

        GlobalInfo.inMenu = inMenu;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Escape"))
        {
            if (!inMenu)
                ShowEscapeMenu();
            else if (!isUpgradeMenu)
                TryHideEscapeMenu();
            else
            {
                TryHideUpgradeMenu();
                ShowEscapeMenu();
            }
        }
        else if (Input.GetButtonDown("Tab"))
        {
            if (!inMenu)
                ShowUpgradeMenu();
            else if (isUpgradeMenu)
                TryHideUpgradeMenu();
            else
            {
                TryHideEscapeMenu();
                ShowUpgradeMenu();
            }
        }
    }

    internal void ShowIntroPaper()
    {
        introPaper.SetActive(true);
    }
    internal void HideIntroPaper()
    {
        introPaper.SetActive(false);
    }

    internal void ShowLoadingScreen()
    {
        loadingScreen.SetActive(true);
    }
    internal void HideLoadingScreen()
    {
        loadingScreen.SetActive(false);
    }

    internal void SetLoadingScreenProgress(float progress)
    {
        Debug.Log("Loading Progress: " + progress);
        loadingSlider.value = progress;
    }

    internal void TryHideUpgradeMenu()
    {
        if (!inMenu)
            return;

        if (isUpgradeMenu)
        {
            MenuToggle(false);
            tooltip.HideTooltipOnMenuToggle();
            UIPanel.SetActive(false);
        }
    }
    internal void TryHideEscapeMenu()
    {
        if (!inMenu)
            return;

        if (!isUpgradeMenu)
        {
            MenuToggle(false);
            tooltip.HideTooltipOnMenuToggle();
            escapePanel.SetActive(false);
        }
    }

    internal void WinGame()
    {
        youWin.SetActive(true);
    }

    private void ShowUpgradeMenu()
    {
        MenuToggle(true);
        isUpgradeMenu = true;
        UIPanel.SetActive(true);
    }
    public void ShowEscapeMenu()
    {
        MenuToggle(true);
        isUpgradeMenu = false;
        escapePanel.SetActive(true);
    }

    internal void MenuToggle(bool targetMenuState)
    {
        inMenu = targetMenuState;
        GlobalInfo.inMenu = targetMenuState;

        mouseLook.SetCursorLock(!inMenu);

        if (inMenu)
        {
            Time.timeScale = 0f;
            MusicManager.Instance.ApplyMasterLowpass();

            SetStartNextWaveText(false);
        }
        else
        {
            Time.timeScale = 1f;
            MusicManager.Instance.ResetMasterLowpass();

            if (!inIntro && EnemyWaveSpawner.Instance.isTDlevel && EnemyWaveSpawner.Instance.allowReady)
                SetStartNextWaveText(true);
        }
    }

    internal void SetTDUI(bool v)
    {
        SetStartNextWaveText(v);
        turretBuildPanel.SetActive(v);
    }

    internal void ShowPopupTextAndPause(string text)
    {
        GameController.Instance.PauseGame();
        orbCollectedPanel.GetComponentInChildren<TextMeshProUGUI>().text = text;
        orbCollectedPanel.SetActive(true);
    }

    internal void ShowBossHpBar(EnemyHealth bossEh)
    {
        bossHpBar.SetActive(true);
        bossHpBarDisplayComponent.EnableBossHpBar(bossEh);
    }
    internal void HideBossHpBar()
    {
        bossHpBar.SetActive(false);
    }

    #region SetText and UpdateText

    internal void UpdateLighthouseHp()
    {
        SetLighthouseHpUI(LighthouseManager.Instance.currentLighthouseHp, LighthouseManager.Instance.maxLighthouseHp);
    }

    internal void SetLighthouseHpUI(int currentHp, int maxHp)
    {
        livesText.text = $"TP: {currentHp} / {maxHp}";
    }

    internal void SetStartNextWaveText(bool state)
    {
        startNextWave.SetActive(state);
    }

    internal void SetManaText(string mana)
    {
        manaText.text = mana;
    }
    internal void SetHealthText(string health)
    {
        healthText.text = health;
    }
    internal void SetMoneyText(string money)
    {
        moneyText.text = money;
    }
    internal void SetWaveIndexText(string waveIndex)
    {
        waveIndexText.text = waveIndex;
    }
    internal void SetKillCountText(string killCount)
    {
        killCountText.text = killCount;
    }
    #endregion SetText

    internal void TryUpdateUIFromSaveFile()
    {
        if (!SaveSystem.GetSaveFileExists())
            return;

        SaveData gameData = SaveSystem.LoadGame();

        SetHealthText(gameData.upgrades[4].ToString());
        SetLighthouseHpUI(gameData.lighthouseHp, gameData.upgrades[1] + 50);
        SetMoneyText(gameData.money.ToString());
        SetWaveIndexText(gameData.wave.ToString());
        SetKillCountText(gameData.killCount.ToString());
    }

    #region Item and Interaction Hints

    internal void ShowItemName(RaycastHit hit)
    {
        itemNameText.text = hit.transform.name;
        itemNameText.gameObject.SetActive(true);
    }

    internal void TryHideItemName()
    {
        if (itemNameText.gameObject.activeSelf)
        {
            itemNameText.gameObject.SetActive(false);
        }
    }

    internal void ShowInteractHint(RaycastHit hit)
    {
        ShowItemName(hit);
        interactText.SetActive(true);
    }

    internal void TryHideInteractHint()
    {
        if (interactText.activeSelf)
        {
            TryHideItemName();
            interactText.SetActive(false);
        }
    }

    internal void ShowItemHint(RaycastHit hit)
    {
        ShowItemName(hit);
        itemPickupText.SetActive(true);
    }
    internal void TryHideItemHint()
    {
        if (itemPickupText.activeSelf)
        {
            TryHideItemName();
            itemPickupText.SetActive(false);
        }
    }

    #endregion

    internal void PlayNotEnoughMoneyAnim()
    {
        notEnoughMoneyAnim.Play("MoneyBlink", -1, normalizedTime: 0f);
    }

    internal void LoseScreen()
    {
        inLostScreen = true;
        ShowMouse();
        youLost.SetActive(true);
    }

    #region Mouse
    internal void ShowMouse()
    {
        mouseLook.SetCursorLock(false);
    }
    internal void HideMouse()
    {
        mouseLook.SetCursorLock(true);
    }
    #endregion Mouse
}