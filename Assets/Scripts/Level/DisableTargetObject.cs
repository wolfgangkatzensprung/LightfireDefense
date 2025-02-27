using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableTargetObject : MonoBehaviour
{
    public GameObject target;

    public void DisableTarget()
    {
        target.SetActive(false);
    }
}
