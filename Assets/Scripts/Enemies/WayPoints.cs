using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoints : MonoBehaviour
{
    public static Transform[] rPoints;
    public static Transform[] lPoints;
    public static Transform[] dPoints;

    [Tooltip("Default Way")]
    public Transform rPointsRoot;
    [Tooltip("2nd Way")]
    public Transform lPointsRoot;
    [Tooltip("Direct Way for Air Push")]
    public Transform dPointsRoot;

    private void Awake()
    {
        rPoints = new Transform[rPointsRoot.childCount];
        for (int i = 0; i < rPoints.Length; i++)
        {
            rPoints[i] = rPointsRoot.GetChild(i);
        }  
        
        lPoints = new Transform[lPointsRoot.childCount];
        for (int i = 0; i < lPoints.Length; i++)
        {
            lPoints[i] = lPointsRoot.GetChild(i);
        }  
        
        dPoints = new Transform[dPointsRoot.childCount];
        for (int i = 0; i < dPoints.Length; i++)
        {
            dPoints[i] = dPointsRoot.GetChild(i);
        }
    }
}