using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RotateLighthouseBeamPivot : MonoBehaviour
{
    public float maxRotationSpeed = 10f;
    float xRotationSpeed;

    float randomXRotationSpeed = 0f;
    float directionChangeTimer = 0f;
    float directionChangeTime = 5f;

    [Tooltip("After this amount of seconds not attacking in Attack mode, LightBeamMode will become Wave mode")]
    public float modeChangeTime = 5f;
    float modeChangeTimer = 0f;

    enum LightBeamMode
    {
        Idle,   // im Kreis
        Wave,   // paranoid herum
        Attack  // beim Angreifen auf Close Range und auf Bosse quer durchs Level
    }
    LightBeamMode beamMode;

    private void Start()
    {
        xRotationSpeed = Random.Range(0f, maxRotationSpeed);

        if (EnemyWaveSpawner.Instance != null)
        {
            EnemyWaveSpawner.Instance.onWaveStart += WaveMode;
            EnemyWaveSpawner.Instance.onWaveFinished += IdleMode;
        }
    }

    private void Update()
    {
        if (beamMode == LightBeamMode.Idle)
        {
            transform.Rotate(0, xRotationSpeed, 0f);
        }
        else if (beamMode == LightBeamMode.Wave)
        {
            directionChangeTimer += Time.deltaTime;

            if (directionChangeTimer > directionChangeTime || transform.rotation.x < -60f || transform.rotation.x > 240f)
            {
                DirectionChange();
            }

            transform.Rotate(randomXRotationSpeed * Time.deltaTime, maxRotationSpeed * Time.deltaTime, 0f);
        }
        else if (beamMode == LightBeamMode.Attack)
        {
            modeChangeTimer += Time.deltaTime;
            if (modeChangeTimer > modeChangeTime)
            {
                beamMode = LightBeamMode.Wave;
                modeChangeTimer = 0f;
            }
        }
    }

    private void DirectionChange()
    {
        xRotationSpeed = Random.Range(0f, maxRotationSpeed);
        randomXRotationSpeed = Random.Range(-xRotationSpeed, xRotationSpeed);
        Debug.Log($"XrotationSpeed: {randomXRotationSpeed}");
        directionChangeTime = Random.Range(3f, 6f);
        directionChangeTimer = 0f;
    }

    internal void SetLookAt(Transform targetTrans)
    {
        beamMode = LightBeamMode.Attack;
        transform.LookAt(targetTrans, Vector3.up);
    }

    void WaveMode()
    {
        beamMode = LightBeamMode.Wave;
    }
    void IdleMode()
    {
        beamMode = LightBeamMode.Idle;
    }
}
