using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AutoDestroyPS : MonoBehaviour
{
    private float timeLeft;

    private void Awake()
    {
        ParticleSystem particles = GetComponent<ParticleSystem>();
        var main = particles.main;
        timeLeft = main.startLifetimeMultiplier + main.duration;
        Destroy(gameObject, timeLeft);
    }
}