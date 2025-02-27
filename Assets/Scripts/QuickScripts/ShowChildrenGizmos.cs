using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowChildrenGizmos : MonoBehaviour
{
    public Color color = Color.cyan;
    public float size = 3f;
    private void OnDrawGizmos()
    {
        Gizmos.color = color;

        foreach(Transform child in transform)
            Gizmos.DrawSphere(child.position, size);
    }
}
