using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedElementUI : MonoBehaviour
{
    public Image[] elementUIimages;

    private void Start()
    {
        ElementalScroll.Instance.onEleChange += UIElementSelection;
        Select(0);
    }

    private void UIElementSelection()
    {
        int selectionIndex = ElementalScroll.Instance.selectionIndex;

        for (int i = 0; i < elementUIimages.Length; i++)
        {
            if (i != selectionIndex)
            {
                Deselect(i);
            }
            else
            {
                Select(i);
            }
        }
    }

    private void Select(int i)
    {
        Color currentEleColor = ElementalScroll.Instance.GetCurrentSpellColor();
        elementUIimages[i].color = currentEleColor;
        elementUIimages[i].GetComponentsInChildren<Image>()[1].color = currentEleColor;
        elementUIimages[i].transform.localScale = new Vector3(.13f, .13f, 1f);
        elementUIimages[i].transform.SetAsLastSibling();
    }

    private void Deselect(int i)
    {
        elementUIimages[i].color = Color.white;
        elementUIimages[i].GetComponentsInChildren<Image>()[1].color = Color.white;
        elementUIimages[i].transform.localScale = new Vector3(.1f, .1f, 1f);
    }
}