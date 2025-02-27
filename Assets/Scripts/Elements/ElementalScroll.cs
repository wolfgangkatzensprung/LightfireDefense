using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalScroll : Singleton<ElementalScroll>
{
    // Reihenfolge: Erde, Feuer, Wasser, Luft
    internal int selectionIndex = 0;

    bool scrolling;
    float scrollingDelay = .3f;

    public delegate void ElementalUIDelegate();
    public ElementalUIDelegate onEleChange;

    internal void ActivateEleScroll()
    {
        PlayerInputManager.Instance.onMouseScroll += ElementalChange;
    }

    void ElementalChange(int mouseScrollDirection)
    {
        if (scrolling)
            return;

        StartCoroutine(ElementalScrolling(mouseScrollDirection));
    }

    internal Color GetCurrentSpellColor()
    {
        Color eleColor = Color.white;

        switch (selectionIndex)
        {
            case 0:
                eleColor = Color.green;
                break;
            case 1:
                eleColor = Color.red;
                break;
            case 2:
                eleColor = new Color(0, .5f, 1f, 1f);
                break;
            case 3:
                eleColor = Color.yellow;
                break;
        }
        return eleColor;
    }

    private IEnumerator ElementalScrolling(int mouseScrollDirection)
    {
        scrolling = true;

        if (selectionIndex + mouseScrollDirection > 3)
        {
            selectionIndex = 0;
        }
        else if (selectionIndex + mouseScrollDirection < 0)
        {
            selectionIndex = 3;
        }
        else
        {
            selectionIndex = Mathf.Max(Mathf.Min(selectionIndex + mouseScrollDirection, 4), 0);
        }

        //Debug.Log($"selectionIndex {selectionIndex}");
        PlayerSpells.Instance.SwitchSpell(selectionIndex);
        onEleChange?.Invoke();

        yield return new WaitForSeconds(scrollingDelay);
        scrolling = false;
    }

    internal void SetElement(int elementIndex)
    {
        selectionIndex = elementIndex;
        PlayerSpells.Instance.SwitchSpell(selectionIndex);
        onEleChange?.Invoke();
    }

    internal Damage.DamageType GetCurrentDamageType()
    {
        Damage.DamageType dmgType = Damage.DamageType.Water;

        switch (selectionIndex)
        {
            case 0:
                dmgType = Damage.DamageType.Earth;
                break;
            case 1:
                dmgType = Damage.DamageType.Fire;
                break;
            case 2:
                dmgType = Damage.DamageType.Water;
                break;
            case 3:
                dmgType = Damage.DamageType.Air;
                break;
        }
        return dmgType;
    }
}
