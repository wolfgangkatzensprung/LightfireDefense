using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ECM.Components;

public class Menu_ContinueButton : MonoBehaviour
{
    public void Continue()
    {
        if (SaveSystem.GetSaveFileExists())
        {
            SceneLoading.Instance.StartGameLoading();

            CloseUI();

            GlobalInfo.Instance.mouseLook.lockCursor = true;
        }
        else
        {
            Debug.Log("NEW GAME");
            GlobalInfo.isNewStart = true;

            CloseUI();

            if (GlobalInfo.Instance.magicWand.activeSelf)
            {
                GlobalInfo.Instance.magicWand.SetActive(false);
            }
            SceneLoading.Instance.StartNewGameLoading();
        }
    }

    private static void CloseUI()
    {
        if (UIManager.Instance != null && !UIManager.Instance.UI.activeSelf)
        {
            UIManager.Instance.UI.SetActive(true);
            UIManager.Instance.inMenu = false;
        }
    }
}