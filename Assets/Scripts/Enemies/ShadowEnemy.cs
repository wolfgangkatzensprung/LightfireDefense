using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
public class ShadowEnemy : MonoBehaviour
{
    public GameObject eyes;
    Rigidbody rb;
    EnemyHealth eh;
    ParticleSystem ps;
    ParticleSystem.MainModule psMain;

    public GameObject ectoPrefab;
    Transform playerTrans;

    public float speed = .1f;
    internal bool chasing;
    float chasingTime = 0f;
    float maxSpeed = 3f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        ps = GetComponentInChildren<ParticleSystem>();
        eh = GetComponent<EnemyHealth>();
    }
    private void Start()
    {
        playerTrans = GlobalInfo.Instance.playerTrans;
        GetComponent<EnemyDmgOnTouch>().onDamage += BisectChasingTime;
    }

    private void Update()
    {
        if (chasing)
            transform.LookAt(playerTrans);
    }

    private void FixedUpdate()
    {
        if (chasing)
        {
            ChasePlayer();
            chasingTime += Time.deltaTime;
        }
        else
        {
            ResetChasingTime();
        }
    }

    internal void ResetChasingTime()
    {
        chasingTime = 0f;
    }

    void BisectChasingTime()
    {
        chasingTime *= .5f;
    }

    private void ChasePlayer()
    {
        rb.MovePosition(transform.position + transform.forward * Mathf.Max(maxSpeed, speed * chasingTime) * Time.deltaTime);
    }

    internal void FadeIn()
    {
        ps.Play();
        eyes.SetActive(true);
    }
    internal void FadeOut()
    {
        transform.position = Vector3.down * 100f;
        eyes.SetActive(false);
    }

    public void DropEcto()
    {
        Instantiate(ectoPrefab, transform.position, Quaternion.identity);
    }

    private void OnDestroy()
    {
        FadeOut();
    }
}