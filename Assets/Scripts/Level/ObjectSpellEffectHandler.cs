using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpellEffectHandler : Singleton<ObjectSpellEffectHandler>
{
    public delegate void SpellEffectDelegate();
    public SpellEffectDelegate onSpellEffect;

    private void Update()
    {
        onSpellEffect?.Invoke();
    }
}
