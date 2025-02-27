using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Triggers a UnityEvent, starts timer and triggers another Unity Event after the timer has reached the final value
/// </summary>
public class TimedEvent : MonoBehaviour
{
    [Tooltip("If true, StartTimedEventSequence() will be triggered on Start()")]
    public bool triggerOnStart;
    [Tooltip("Time until second event (for example reenabling a deactivated object/script or vise versa)")]
    public float time = 5f;
    [Tooltip("First UnityEvent that starts the timer")]
    public UnityEvent startEvent;
    [Tooltip("Timed UnityEvent when timer has reached final value")]
    public UnityEvent timedEvent;

    private void Start()
    {
        if (triggerOnStart)
            StartTimedEventSequence();
    }

    public void StartTimedEventSequence()
    {
        startEvent.Invoke();
        Invoke("FinalEvent", time);
    }


    void FinalEvent()
    {
        timedEvent.Invoke();
    }
}
