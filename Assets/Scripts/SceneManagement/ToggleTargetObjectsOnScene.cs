using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleTargetObjectsOnScene : MonoBehaviour
{
    [Tooltip("Enable GameObject[] when this Scene is loaded. Disables GameObject[] when different Scene is loaded.")]
    public string sceneName = "TD Level";

    public GameObject[] toggleObjects;

    private void Start()
    {
        SceneLoading.Instance.onSceneLoadedAsync += ToggleGameObject;
    }

    private void ToggleGameObject(string sceneName)
    {
        bool toggle = (sceneName == this.sceneName);

        foreach (GameObject go in toggleObjects)
        {
            go.SetActive(toggle);
        }
    }
}