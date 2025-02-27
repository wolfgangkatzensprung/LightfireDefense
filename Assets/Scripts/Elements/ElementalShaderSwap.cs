using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementalShaderSwap : MonoBehaviour
{
    public Material elementalMaterial;

    public float emissionIntensity = 55f;

    [Header("Elemental Colors")]
    public Color eleGreen = Color.green;
    public Color eleRed = Color.red;
    public Color eleBlue = new Color(0, .5f, 1f, 1f);
    public Color eleYellow = Color.yellow;

    private void Start()
    {
        ElementalScroll.Instance.onEleChange += UpdateElementalShader;
        UpdateElementalShader();
    }

    private void UpdateElementalShader()
    {
        //Debug.Log("New Ele Color");

        Color eleColor = new Color();

        switch(ElementalScroll.Instance.selectionIndex)
        {
            case 0:
                eleColor = eleGreen;
                break;
            case 1:
                eleColor = eleRed;
                break;
            case 2:
                eleColor = eleBlue;
                break;
            case 3:
                eleColor = eleYellow;
                break;
        }

        elementalMaterial.SetColor("_Color", eleColor * emissionIntensity);
    }
}
