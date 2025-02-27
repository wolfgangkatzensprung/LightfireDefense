using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ECM.Components;
using UnityEngine.SceneManagement;

public class GameController : Singleton<GameController>
{
    public GameObject introPrefab;
    CharacterMovement cm;

    internal bool gamePaused;

    public delegate void GameLoadedDelegate();

    public GameLoadedDelegate onGameLoaded;

    public delegate void StartNewGameDelegate();
    public StartNewGameDelegate onStartNewGame;


    private void Start()
    {
        cm = GetComponent<CharacterMovement>();

        EnemyWaveSpawner.Instance.onWaveStart += TryFinalWave;
    }

    internal void PauseGame()
    {
        gamePaused = true;
        StopGameLogic();
        Time.timeScale = 0f;
    }
    internal void UnpauseGame()
    {
        StartGameLogic();
        Time.timeScale = 1f;
        gamePaused = false;
    }

    public void StartNewGame()
    {
        UIManager.Instance.inIntro = true;
        GlobalInfo.inIntro = true;

        UpgradeManager.Instance.ResetUpgrades();

        PlayerMoney.Instance.SetMoney(0);
        PlayerHealth.Instance.SetHealth(100);
        PlayerMana.Instance.SetMana(25);

        PlayerHealth.Instance.ResetOrangeBuff();
        PlayerMana.Instance.ResetBlueBuff();

        LighthouseManager.Instance.SetDefaults();

        UIManager.Instance.TryHideUpgradeMenu();
        UIManager.Instance.TryHideEscapeMenu();
        EnemyWaveSpawner.Instance.allowReady = false;
        EnemyWaveSpawner.Instance.SetWaveIndex(1);
        UIManager.Instance.SetStartNextWaveText(false);
        UIManager.Instance.TryUpdateUIFromSaveFile();

        UIManager.Instance.HideMouse();
        UIManager.Instance.MenuToggle(false);

        onStartNewGame?.Invoke();

        SaveSystem.SaveGame();
    }

    internal void SetPlayerPositionAndRotation(Vector3 position, Quaternion rotation)
    {
        GlobalInfo.Instance.playerTrans.position = position;
        GlobalInfo.Instance.playerTrans.rotation = rotation;


        //if (cm != null)
        //    cm.rotation = rotation;
        //else
        //{
        //    cm = GetComponent<CharacterMovement>();
        //    if (cm != null)
        //        cm.rotation = rotation;
        //}
    }

    internal static void MoveObjectToActiveScene(GameObject gameObject)
    {
        SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
    }  
    internal static void MoveObjectToCurrentLevelScene(GameObject gameObject)
    {
        SceneManager.MoveGameObjectToScene(gameObject, SceneLoading.Instance.currentLevelScene);
    }

    internal void FinishIntro()
    {
        UIManager.Instance.inIntro = false;
        GlobalInfo.inIntro = false;
        GlobalInfo.isNewStart = false;

        EnemyWaveSpawner.Instance.allowReady = true;
        EnemyWaveSpawner.Instance.isInWave = false;

        UIManager.Instance.MenuToggle(false);
        UIManager.Instance.SetStartNextWaveText(true);

        StartGameLogic();
        Time.timeScale = 1f;
    }

    /// <summary>
    /// Continue Game by pressing Continue Button in Menu
    /// </summary>
    public void ContinueGame()
    {
        LoadGame();

        UIManager.Instance.MenuToggle(false);

        onGameLoaded?.Invoke();
    }

    public static void StopGameLogic()
    {
        PlayerShooting.Instance.enabled = false;
        PlayerSpells.Instance.enabled = false;
        UIManager.Instance.enabled = false;
    }

    public static void StartGameLogic()
    {
        UIManager.Instance.enabled = true;
        PlayerSpells.Instance.enabled = true;
        PlayerShooting.Instance.enabled = true;
    }

    private static void LoadGame()
    {
        SaveData data = SaveSystem.LoadGame();

        PlayerHealth.Instance.LoadHpAndOrangeBuff();
        PlayerExp.Instance.SetExp(data.exp);
        PlayerExp.Instance.SetLevels(data.levels);
        PlayerMoney.Instance.SetMoney(data.money);
        UpgradeManager.Instance.LoadUpgrades(data.upgrades);

        EnemyWaveSpawner.Instance.SetWaveIndex(data.wave);

        SphereKeys.LoadKeys(data.keys);

        Instance.LoadSavedObjects();
    }

    internal void SpawnObject(GameObject objectPrefab, Vector3 spawnPosition)
    {
        Instantiate(objectPrefab, spawnPosition, Quaternion.identity);
    }

    public void TryFinalWave()
    {
        if (EnemyWaveSpawner.Instance.GetWaveIndex() == 50)
        {
            SpecialWaves.Instance.DoEndBossFight();
        }
        else if (EnemyWaveSpawner.Instance.GetWaveIndex() > 50)
        {
            UIManager.Instance.WinGame();
        }
    }

    internal void LoadSavedObjects()
    {
        Debug.Log("Load Saved Objects");
        TrapManager.Instance.LoadTraps();
        TurretManager.Instance.LoadTurrets();
        GardenManager.Instance.LoadBushes();
    }

    public void WinGame()
    {
        Debug.Log("GAME WON !!!!!!!");
    }

    public void LoseGame()
    {
        UIManager.Instance.LoseScreen();
    }

    internal void DeactivateTD()
    {
        EnemyHandler.Instance.ClearEnemies();
        EnemyWaveSpawner.Instance.isTDlevel = false;
        EnemyWaveSpawner.Instance.isInWave = false;

        UIManager.Instance.SetTDUI(false);
    }
    internal void ActivateTD()
    {
        EnemyWaveSpawner.Instance.isTDlevel = true;
        EnemyWaveSpawner.Instance.isInWave = true;
        EnemyHandler.Instance.allowCompleteWave = true;

        UIManager.Instance.SetTDUI(true);

    }
}