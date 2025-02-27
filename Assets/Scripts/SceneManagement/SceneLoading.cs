using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoading : Singleton<SceneLoading>
{
    public bool isLoadedAsync;

    public Scene currentLevelScene;

    public delegate void AsyncSceneLoadedDelegate(string sceneName);
    public AsyncSceneLoadedDelegate onSceneLoadedAsync;

    public override void Awaken()
    {
        Initialize();
    }

    private void Initialize()
    {
        Debug.Log("Initialize()");

#if UNITY_EDITOR
        currentLevelScene = SceneManager.GetSceneByName("TD Level");
        return;
#endif
        LoadStartMenu();
    }

    private void LoadStartMenu()
    {
        SceneManager.LoadScene("StartMenu", LoadSceneMode.Additive);
    }

    public void StartNewGameLoading()
    {
        SaveSystem.DeleteSaveGame();
        StartCoroutine(InitializeNewGameRoutine());
    }

    public void StartGameLoading()
    {
        StartCoroutine(InitializeGameRoutine());
    }

    IEnumerator InitializeGameRoutine()
    {
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync("StartMenu");
        while (!asyncUnload.isDone)
        {
            Debug.Log("Unloading StartMenu with " + asyncUnload.progress + " progress");
            yield return null;
        }

        if (!SceneManager.GetSceneByName("UI").isLoaded)
        {
            AsyncOperation asyncLoadUI = SceneManager.LoadSceneAsync("UI", LoadSceneMode.Additive);
            while (!asyncLoadUI.isDone)
            {
                yield return null;
            }
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("TD Level", LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            Debug.Log("Loading ... " + asyncLoad.progress);
            if (UIManager.Instance != null)
                UIManager.Instance.SetLoadingScreenProgress(asyncLoad.progress);
            yield return null;
        }
        currentLevelScene = SceneManager.GetSceneByName("TD Level");
        SceneManager.SetActiveScene(currentLevelScene);
        onSceneLoadedAsync?.Invoke("TD Level");

        Debug.Log($"Initializing Game Done. currentLevelScene is {currentLevelScene}");

        GameController.Instance.ContinueGame();
    }

    IEnumerator InitializeNewGameRoutine()
    {
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync("StartMenu");
        while (!asyncUnload.isDone)
        {
            Debug.Log("Unloading StartMenu with " + asyncUnload.progress + " progress");
            yield return null;
        }
        Debug.Log("StartMenu unloaded: " + asyncUnload.isDone);

        if (!SceneManager.GetSceneByName("UI").isLoaded)
        {
            AsyncOperation asyncLoadUI = SceneManager.LoadSceneAsync("UI", LoadSceneMode.Additive);
            while (!asyncLoadUI.isDone)
            {
                yield return null;
            }
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("TD Level", LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            Debug.Log("Loading TD Level ... " + asyncLoad.progress);
            UIManager.Instance.SetLoadingScreenProgress(asyncLoad.progress);
            yield return null;
        }
        currentLevelScene = SceneManager.GetSceneByName("TD Level");
        SceneManager.SetActiveScene(currentLevelScene);
        onSceneLoadedAsync?.Invoke("TD Level");

        Debug.Log("Initializing NewGame Done.");

        GameController.Instance.StartNewGame();
    }

    internal void LoadLevel(string sceneName)
    {
        Debug.Log("START LOADING: " + sceneName);

        EnemyWaveSpawner.Instance.TryCancelWaveSpawn();

        StartCoroutine(LoadNextLevelSceneAsync(sceneName, LoadSceneMode.Additive));
    }

    IEnumerator LoadNextLevelSceneAsync(string sceneName, LoadSceneMode mode)
    {
        isLoadedAsync = false;

        //ReassignCurrentLevelScene();      deaktiviert weil Main Scene active sein kann

        Debug.Log($"Starting AsyncUnload of {currentLevelScene.name}");
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(currentLevelScene, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);

        while (!asyncUnload.isDone)
        {
            Debug.Log("UNLOADING");
            yield return null;
        }

        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, mode);
        UIManager.Instance.ShowLoadingScreen();

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            Debug.Log("LOADING: " + sceneName);
            UIManager.Instance.SetLoadingScreenProgress(asyncLoad.progress);
            yield return null;
        }
        Debug.Log("FINISHED LOADING: " + sceneName);
        UIManager.Instance.HideLoadingScreen();

        currentLevelScene = SceneManager.GetSceneByName(sceneName);

        if (sceneName != "TD Level")
        {
            GameController.Instance.DeactivateTD();
        }
        else
        {
            GameController.Instance.ActivateTD();
            GameController.Instance.LoadSavedObjects();
        }

        SceneManager.SetActiveScene(currentLevelScene);
        isLoadedAsync = true;
        onSceneLoadedAsync?.Invoke(sceneName);
    }

    private void ReassignCurrentLevelScene()
    {
        currentLevelScene = SceneManager.GetActiveScene();
        Debug.Log($"currentLevelScene has been reassigned: {currentLevelScene.name}");
    }

    internal void BackToMenu()
    {
        MusicManager.Instance.PlayMusic(MusicManager.Instance.menuMusic);
        GlobalInfo.inMenu = true;

        ItemHandler.Instance.ClearItems();
        GameController.Instance.DeactivateTD();

        StartCoroutine(BackToMenuRoutine());
    }

    IEnumerator BackToMenuRoutine()
    {
        // Unload

        yield return new WaitForEndOfFrame();

        AsyncOperation asyncUnloadLevel = SceneManager.UnloadSceneAsync(currentLevelScene, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        while (!asyncUnloadLevel.isDone)
        {
            yield return null;
        }

        // Load

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("StartMenu", LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            Debug.Log("LOADING: " + "StartMenu");
            yield return null;
        }
        Debug.Log("FINISHED LOADING: " + "StartMenu");

        onSceneLoadedAsync?.Invoke("StartMenu");
    }

    internal void Respawn()
    {
        Debug.Log("Respawn()");
        UIManager.Instance.TryUpdateUIFromSaveFile();

        UIManager.Instance.SetStartNextWaveText(true);

        LoadLevel("TD Level");
    }

    internal void ReloadGame()
    {
        StartCoroutine(ReloadGameRoutine());
    }

    IEnumerator ReloadGameRoutine()
    {
        yield return new WaitForEndOfFrame();

        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(currentLevelScene, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        while (!asyncUnload.isDone)
        {
            yield return null;
        }
        StartCoroutine(LoadNextLevelSceneAsync("StartMenu", LoadSceneMode.Additive));
    }
}