using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePointAntiClip : MonoBehaviour
{
    Vector3 startLocalPos;
    Vector3 earthLocalPos;

    private void Start()
    {
        startLocalPos = transform.localPosition;
        earthLocalPos = startLocalPos - Vector3.forward * 2;
        ElementalScroll.Instance.onEleChange += SetFirePointPosition;
    }

    void SetFirePointPosition()
    {
        if (ElementalScroll.Instance.selectionIndex != 0)
            transform.localPosition = startLocalPos;
        else
            transform.localPosition = earthLocalPos;

    }
}
