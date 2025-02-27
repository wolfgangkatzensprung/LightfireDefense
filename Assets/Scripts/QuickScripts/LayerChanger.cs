using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerChanger : MonoBehaviour
{
    public int targetLayer;

    public void SetLayer()
    {
        gameObject.layer = targetLayer;
    }
}
