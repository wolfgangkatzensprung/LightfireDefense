using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVolumeSelection : MonoBehaviour
{
    public GameObject globalVolumeTd;
    public GameObject globalVolume;

    private void Start()
    {
        SceneLoading.Instance.onSceneLoadedAsync += SelectGlobalVolume;
    }

    void SelectGlobalVolume(string sceneName)
    {
        if (sceneName == "TD Level")
        {
            globalVolume.SetActive(false);
            globalVolumeTd.SetActive(true);
        }
        else
        {
            globalVolume.SetActive(true);
            globalVolumeTd.SetActive(false);
        }
    }
}