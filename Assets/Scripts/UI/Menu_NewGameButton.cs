using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_NewGameButton : MonoBehaviour
{
    public GameObject newGamePrompt;

    public void TryOpenNewGamePrompt()
    {
        if (!SaveSystem.GetSaveFileExists())
            StartNewGame();
        else
            newGamePrompt.SetActive(true);
    }

    public void CloseNewGamePrompt()
    {
        newGamePrompt.SetActive(false);
    }

    public void StartNewGame()
    {
        Debug.Log("NEW GAME");
        GlobalInfo.isNewStart = true;
        if (UIManager.Instance != null && !UIManager.Instance.UI.activeSelf)
        {
            UIManager.Instance.UI.SetActive(true);
            UIManager.Instance.inMenu = false;
        }
        if (GlobalInfo.Instance.magicWand.activeSelf)
        {
            GlobalInfo.Instance.magicWand.SetActive(false);
        }
        SceneLoading.Instance.StartNewGameLoading();
    }
}