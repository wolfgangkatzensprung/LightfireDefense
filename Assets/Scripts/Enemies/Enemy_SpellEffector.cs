using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_SpellEffector : MonoBehaviour, ISpellEffectListener
{
    Rigidbody rb;
    Enemy_TD td;
    EnemyHealth eh;

    [Tooltip("Vector3.up Force applied to Rigidbody on Air Spell")]
    public float airForce = 55f;

    [Tooltip("Drag applied to Rigidbody on Earth Spell")]
    public float earthDrag = 77f;
    float startDrag;
    bool startUseGravity;

    [Tooltip("Extra damage applied to EnemyHealth on Fire Spell")]
    public int fireBonusDamage = 1;

    // Start Position for Time Spell Effect
    Vector3 startPosition;

    // currently active Spell Effects with timer
    Dictionary<SpellField.SpellType, float> activeSpellEffects = new Dictionary<SpellField.SpellType, float>() {
        {SpellField.SpellType.Air, 0f},
        {SpellField.SpellType.Earth, 0f},
        {SpellField.SpellType.Fire, 0f},
        {SpellField.SpellType.Water, 0f},
        {SpellField.SpellType.Time, 0f}
    };

    [Tooltip("Maximum time in seconds that spell effect is applied in the dictionary")]
    public float maxSpellEffectTime = 3f;

    internal bool immune { get; set; }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        eh = GetComponent<EnemyHealth>();
        startPosition = transform.position;
        startDrag = rb.drag;
        startUseGravity = rb.useGravity;
    }

    private void Update()
    {
        for(int i = 0; i < activeSpellEffects.Count; i++)
        {
            if (activeSpellEffects[(SpellField.SpellType)i] > 0)
            {
                activeSpellEffects[(SpellField.SpellType)i] -= Time.deltaTime;
                SpellEffectSwitch((SpellField.SpellType)i);
            }
            else if (activeSpellEffects[(SpellField.SpellType)i] <= 0)
            {
                SpellCancelSwitch((SpellField.SpellType)i);
                activeSpellEffects[(SpellField.SpellType)i] = 0;
            }
        }
    }

    private void SpellCancelSwitch(SpellField.SpellType spellType)
    {
        switch(spellType)
        {
            case SpellField.SpellType.Earth:
                StopEarthEffect();
                break;
            case SpellField.SpellType.Water:
                StopWaterEffect();
                break;
        }
    }

    private void SpellEffectSwitch(SpellField.SpellType spellType)
    {
        switch (spellType)
        {
            case SpellField.SpellType.Air:
                AirEffect();
                break;
            case SpellField.SpellType.Earth:
                StartEarthEffect();
                break;
            case SpellField.SpellType.Fire:
                FireEffect();
                break;
            case SpellField.SpellType.Water:
                StartWaterEffect();
                break;
            case SpellField.SpellType.Time:
                TimeEffect();
                break;
        }
    }

    public void ApplySpellEffect(SpellField.SpellType spellType)
    {
        if (!immune)
            SpellFieldSwitch(spellType);
    }

    private void SpellFieldSwitch(SpellField.SpellType spellType)
    {
        switch (spellType)
        {
            case SpellField.SpellType.Air:
                activeSpellEffects[SpellField.SpellType.Air] = maxSpellEffectTime;
                break;
            case SpellField.SpellType.Earth:
                activeSpellEffects[SpellField.SpellType.Earth] = maxSpellEffectTime;
                break;
            case SpellField.SpellType.Fire:
                activeSpellEffects[SpellField.SpellType.Fire] = maxSpellEffectTime;
                break;
            case SpellField.SpellType.Water:
                activeSpellEffects[SpellField.SpellType.Water] = maxSpellEffectTime;
                break;
            case SpellField.SpellType.Time:
                activeSpellEffects[SpellField.SpellType.Time] = maxSpellEffectTime;
                break;
        }
    }

    private void AirEffect()
    {
        Debug.Log("AirEffect: Force up");
        rb.AddForce(airForce * Vector3.up, ForceMode.Impulse);
    }

    private void StartEarthEffect()
    {
        Debug.Log("EarthEffect: Drag");
        rb.drag = earthDrag;
    }
    private void StopEarthEffect()
    {
        rb.drag = startDrag;
    }

    private void FireEffect()
    {
        Debug.Log("FireEffect: Burn");
        eh.TryApplyDamage(fireBonusDamage, Damage.DamageType.Fire);
    }

    private void StartWaterEffect()
    {
        Debug.Log("WaterEffect: Float");
        rb.useGravity = false;
    }
    private void StopWaterEffect()
    {
        rb.useGravity = startUseGravity;
    }

    private void TimeEffect()
    {
        transform.position = startPosition;
        if (td != null)
            td.ResetWayPointIndex();
    }
}
