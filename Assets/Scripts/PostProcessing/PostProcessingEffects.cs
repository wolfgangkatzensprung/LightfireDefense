using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
public class PostProcessingEffects : MonoBehaviour
{
    public Volume vol;


    static float damagedTimer = 0f;    // neg timer


    private void Update()
    {
        if(damagedTimer > 0f)
        {
            damagedTimer -= Time.deltaTime;
            LerpColor();
        }
    }

    private void LerpColor()
    {
        vol.weight = Mathf.Lerp(0f, 1f, damagedTimer);
    }

    public static void DamagedEffect()
    {
        damagedTimer = 1f;
    }
}
