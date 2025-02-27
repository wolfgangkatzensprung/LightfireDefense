using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGameButton : MonoBehaviour
{
    public void QuitGame()
    {
        Application.Quit();
    }

    public void QuitToMenu()
    {
        UIManager.Instance.TryHideEscapeMenu();
        UIManager.Instance.UI.SetActive(false);
        EnemyWaveSpawner.Instance.TryCancelWaveSpawn();
        SceneLoading.Instance.BackToMenu();
    }

    public void QuitToMenuAndDelSave()
    {
        UIManager.Instance.TryHideEscapeMenu();
        UIManager.Instance.UI.SetActive(false);
        EnemyWaveSpawner.Instance.TryCancelWaveSpawn();
        SaveSystem.DeleteSaveGame();
        SceneLoading.Instance.BackToMenu();
    }

    public void SaveGame()
    {
        SaveSystem.SaveGame();
    }
}