using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEventOnDestroy : MonoBehaviour
{
    public UnityEvent eventToTrigger;

    private void OnDestroy()
    {
        eventToTrigger.Invoke();
    }
}
