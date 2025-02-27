using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LighthouseBeam : MonoBehaviour
{
    public Light light;

    public LayerMask groundLayer;
    RaycastHit hit;

    public float minDistance = 3f;
    public float maxDistance = 33f;

    Vector3 currentVelocity = Vector3.zero;
    [Tooltip("Time for smoothing to reach destination scale")]
    public float smoothTime = .1f;

    [Tooltip("Z Offset of Raycast so it starts a little bit in front of the Lighthouse")]
    public float raycastOffset = 3f;

    private void Start()
    {
        minDistance = maxDistance * .5f;
    }

    private void Update()
    {
        if (GlobalInfo.inIntro)
        {
            transform.localScale = new Vector3(5f, 5f, 5f);
        }
        else if (!GlobalInfo.inMenu && Physics.Raycast(transform.position + raycastOffset * -transform.forward, -transform.up, out hit, groundLayer))
        {
            float  xzScale = Mathf.Min(Mathf.Max(3f, (maxDistance / hit.distance) * 6f), maxDistance);
            Vector3 targetScale = new Vector3(xzScale, Mathf.Min(maxDistance, Mathf.Max((hit.distance + raycastOffset) * .2f, minDistance)), xzScale);
            Vector3 smoothScale = Vector3.SmoothDamp(transform.localScale, targetScale, ref currentVelocity, smoothTime, Mathf.Infinity, Time.deltaTime);

            transform.localScale = smoothScale;
            light.range = smoothScale.y * 5f + 3f;
        }
    }
}