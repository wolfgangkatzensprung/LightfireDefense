using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    public void PauseTheGame()
    {
        GameController.StopGameLogic();
        Time.timeScale = 0f;
    }
    public void ContinueTheGame()
    {
        GameController.StartGameLogic();
        Time.timeScale = 1f;
    }
}