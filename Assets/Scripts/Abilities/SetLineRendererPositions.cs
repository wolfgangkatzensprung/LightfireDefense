using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLineRendererPositions : MonoBehaviour
{
    [Tooltip("Positionsanzahl muss Childanzahl entsprechen!")]
    public LineRenderer lr;
    private void Start()
    {
        return;

        for (int i = 0; i < lr.positionCount; i++)
        {
            Vector3 pointPosition = transform.GetChild(i).position;
            lr.SetPosition(i, pointPosition);
        }

        SceneLoading.Instance.onSceneLoadedAsync += SetLinesVisible; 
    }

    public void SetLinesVisible(string sceneName)
    {
        return;

        if (sceneName == "TD Level")
        {
            lr.enabled = true;
        }
        else
        {
            lr.enabled = false;
        }
    }
}