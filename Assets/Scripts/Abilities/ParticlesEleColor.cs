using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesEleColor : MonoBehaviour
{
    [Tooltip("Erde Feuer Wasser Luft")]
    public Material[] matPrefabs;
    Renderer rend;

    private void Start()
    {
        rend = GetComponent<Renderer>();

        ElementalScroll.Instance.onEleChange += SwapParticleColor;
    }

    void SwapParticleColor()
    {
        rend.material = matPrefabs[ElementalScroll.Instance.selectionIndex];
    }
}