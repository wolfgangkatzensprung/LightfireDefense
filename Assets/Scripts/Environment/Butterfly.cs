using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butterfly : MonoBehaviour
{
    public ParticleSystem shadowParticleSystem;
    ParticleSystem.MainModule psMain;

    public GameObject eyeL;
    public GameObject eyeR;

    public float speed = 1f;

    internal Vector3 direction = Vector3.forward;

    internal bool isBeingEaten; // true during DinnerTime routine

    private void Start()
    {
        psMain = shadowParticleSystem.main;
    }

    internal void Fly()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    internal void FadeInShadow()
    {
        psMain.loop = true;
        shadowParticleSystem.Play();
        eyeL.SetActive(true);
        eyeR.SetActive(true);
    }
    internal void FadeOutShadow()
    {
        psMain.loop = false;
        eyeL.SetActive(false);
        eyeR.SetActive(false);
    }
}