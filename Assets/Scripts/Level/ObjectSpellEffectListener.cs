using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectSpellEffectListener : MonoBehaviour, ISpellEffectListener
{
    public UnityEvent spellEffectEvent;

    [Tooltip("Event will exclusively trigger on specified spellType")]
    public bool exclusiveSpellType = false;
    [Tooltip("Only works if exclusiveSpellType is true")]
    public SpellField.SpellType specifiedSpellType;

    public virtual void ApplySpellEffect(SpellField.SpellType spellType)
    {
        Debug.Log($"ApplySpellEffect to {gameObject.name}");
        if (exclusiveSpellType)
        {
            if (spellType.Equals(specifiedSpellType))
            {
                spellEffectEvent?.Invoke();
                Debug.Log("Spell Effect Invoked");
                return;
            }
        }
        else
        {
            spellEffectEvent?.Invoke();
            Debug.Log("Spell Effect Invoked");
        }
    }
}
