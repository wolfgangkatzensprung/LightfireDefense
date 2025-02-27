using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(LineRenderer))]
public class AirAttackLineRenderer : MonoBehaviour
{
    public LineRenderer lr;
    MainCamRaycast mcr;

    bool lrActive;
    Vector3[] linePositions = new Vector3[2];

    float defaultDistance = 50f;

    float timer = 0f;
    // Time until linePositions[1] will be changed to new terrain hit
    float swapTime = .1f;

    Vector3 startPoint;
    Vector3 endPoint;
    Vector3 startPointOffset;
    Vector3 endPointOffset;

    private void Start()
    {
        PlayerInputManager.Instance.onStopAttack += StopLineRenderer;
        ElementalScroll.Instance.onEleChange += StopLineRenderer;
        mcr = MainCamRaycast.Instance;
    }

    private void Update()
    {
        if (!lrActive)
        {
            timer = 0f;
            return;
        }

        timer += Time.deltaTime;

        CalculateLinePositions();
        lr.SetPositions(linePositions);
    }

    private void CalculateLinePositions()
    {        
        linePositions[0] = transform.position;

        // fuer -z und +z jeweils pos 1 in richtung transform.forward von pos 0 halten

        if (mcr.aimingAtAnything)
        {
            SetEndPoint();
        }
        else if (timer > swapTime)
        {
            Brzzz();
        }

    }

    private void SetEndPoint()
    {
        Vector3 point = mcr.GetLastHit().point;
        endPoint = point;

        if (transform.forward.z < 0 && point.z > transform.position.z)
        {
            endPoint = transform.position - transform.forward;
        }
        else if (transform.forward.z > 0 && point.z < transform.position.z)
        {
            endPoint = transform.position + transform.forward;
        }

        linePositions[1] = endPoint;
    }

    private void Brzzz()
    {
        float rnd = Random.Range(-1f, 1f);
        float perlin = Mathf.PerlinNoise(transform.position.x, transform.position.z);

        startPointOffset = new Vector3(rnd * perlin, 0f, Mathf.Max(0, 0.1f * rnd * perlin));
        endPointOffset = new Vector3(Mathf.PerlinNoise(transform.position.x, rnd), 0.1f, Mathf.PerlinNoise(rnd, transform.position.y));

        startPoint = transform.position + startPointOffset;
        endPoint = transform.position + mcr.facingDirection.normalized * defaultDistance + endPointOffset;

        linePositions[0] = startPoint;
        linePositions[1] = endPoint;

        timer = 0f;
    }

    internal void StartLineRenderer()
    {
        lr.enabled = true;
        lrActive = true;
    }

    internal void StopLineRenderer()
    {
        lrActive = false;
        lr.enabled = false;
    }
}
