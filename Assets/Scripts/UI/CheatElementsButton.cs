using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatElementsButton : MonoBehaviour
{
    [Tooltip("Cheat Button will set all elemental levels to this value")]
    public int destinationLevels = 5;
    public void GetDemElements()
    {
        int[] levels = new int[PlayerExp.Instance.level.Length];
        for (int i = 0; i < levels.Length; i++)
        {
            levels[i] = destinationLevels;
        }
        PlayerExp.Instance.SetLevels(levels);
    }
}