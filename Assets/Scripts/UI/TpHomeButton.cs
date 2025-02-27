using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TpHomeButton : MonoBehaviour
{
    string targetLocation = "TD Level";

    public void TpHome()
    {
        SaveSystem.SaveGame();
        Debug.Log($"UsePortal() {targetLocation}");

        SceneLoading.Instance.LoadLevel(targetLocation);
    }
}
