using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightEleChange : MonoBehaviour
{
    Light light;

    private void Start()
    {
        light = GetComponent<Light>();
        ElementalScroll.Instance.onEleChange += SwapLightColor;
    }

    void SwapLightColor()
    {
        light.color = ElementalScroll.Instance.GetCurrentSpellColor();
    }
}
