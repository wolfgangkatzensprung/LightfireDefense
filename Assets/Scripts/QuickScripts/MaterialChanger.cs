using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChanger : MonoBehaviour
{
    Renderer rend;

    public Material targetMaterial;

    private void Start()
    {
        rend = GetComponent<Renderer>();
    }

    public void SetTargetMaterial()
    {
        rend.material = targetMaterial;
    }
}
