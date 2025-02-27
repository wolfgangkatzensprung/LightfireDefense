using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableTargetObject : MonoBehaviour
{
    public GameObject target;

    public void EnableTarget()
    {
        target.SetActive(true);
    }
}
