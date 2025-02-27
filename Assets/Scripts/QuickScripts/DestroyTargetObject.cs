using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTargetObject : MonoBehaviour
{
    public GameObject target;

    public void DestroyTarget()
    {
        Destroy(target);
    }
}
