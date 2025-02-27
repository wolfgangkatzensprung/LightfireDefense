using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartEleScrollOnEnable : MonoBehaviour
{
    private void OnEnable()
    {
        ElementalScroll.Instance.ActivateEleScroll();
    }
}