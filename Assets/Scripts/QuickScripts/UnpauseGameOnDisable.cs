using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnpauseGameOnDisable : MonoBehaviour
{
    private void OnDisable()
    {
        if (GameController.Instance != null)
            GameController.Instance.UnpauseGame();
    }
}
