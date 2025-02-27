using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneToRecord : MonoBehaviour
{
    public string sceneName = "TD Level";
    private void Start()
    {
#if UNITY_EDITOR
        return;
#endif
        SceneManager.LoadScene(sceneName);
    }
}
