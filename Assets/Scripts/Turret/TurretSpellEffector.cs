using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Turret))]
public class TurretSpellEffector : MonoBehaviour, ISpellEffectListener
{
    Turret tur;
    Renderer[] renderers;

    [Tooltip("Barrel Renderer Reference")]
    public Renderer barrelRenderer;

    [Tooltip("0 = Blue, 1 = Red, 2 = Yellow, 3 = Green")]
    public Material[] materials;


    private void Start()
    {
        tur = GetComponent<Turret>();
        renderers = GetComponentsInChildren<Renderer>();
    }

    public void ApplySpellEffect(SpellField.SpellType spellType)
    {
        if (spellType != SpellField.SpellType.Time && spellType != SpellField.SpellType.Arcane)
        {
            Debug.Log($"Apply {spellType} effect");
            tur.turretElement = (Turret.TurretElement)(spellType + 1);
            barrelRenderer.material = materials[(int)spellType];
            //for (int i = 0; i < renderers.Length; i++)
            //{
            //    renderers[i].material = materials[(int)spellType];
            //}
        }
    }
}
