using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LightRadiusHandler : MonoBehaviour
{
    Transform playerTrans;

    [Tooltip("Light Radius Rendering Volume")]
    public Volume lightRadiusVolume;

    [Tooltip("Lighthouse Range Indicator Sphere")]
    public GameObject rangeIndicator;

    [Tooltip("Lighthouse Collider with only-Shadow Collision")]
    public SphereCollider shadowCollider;

    public static bool playerInside = true;     // inside light radius

    internal float outsideTimer = 0f;
    Vector3 startPos = new Vector3();
    Vector3 targetPos = new Vector3();
    float lerpTimer = 0f;

    private void Start()
    {
        playerTrans = GlobalInfo.Instance.playerTrans;
        startPos = transform.root.position;
    }

    private void OnEnable()
    {
        if (LighthouseManager.Instance == null) // wenn keine Main Scene aktiv ist
        {
            transform.root.position += Vector3.up * 7f;
            enabled = false;
            return;
        }

        ApplyRadius();
        LighthouseManager.Instance.onRadiusChange += ApplyRadius;
    }

    private void Update()
    {
        if (!playerInside)
        {
            outsideTimer += Time.deltaTime;

            if (Vector3.Distance(transform.position, playerTrans.position) < LighthouseManager.Instance.lighthouseRange)
            {
                playerInside = true;
            }
        }
        else if (playerInside && !(Vector3.Distance(transform.position, playerTrans.position) < LighthouseManager.Instance.lighthouseRange))
        {
            playerInside = false;
        }

        if (lerpTimer < 1f)
        {
            HeightLerp();
            lerpTimer += Time.unscaledDeltaTime;
        }
    }

    internal void ApplyRadius() // Set LightRadius, ShadowCollider Radius, Height
    {
        float r = LighthouseManager.Instance.lighthouseRange;
        lightRadiusVolume.GetComponent<SphereCollider>().radius = r;
        rangeIndicator.transform.localScale = new Vector3(r, r, r);
        shadowCollider.radius = r;
        StartHeightLerp(LighthouseManager.Instance.lighthouseHeight * Vector3.up);
    }

    void StartHeightLerp(Vector3 height)
    {
        //transform.root.position = transform.root.position + height - Vector3.up;    // root transform to start position of incoming lerp
        targetPos = startPos + height;
        lerpTimer = 0f;

    }
    void HeightLerp()
    {
        transform.root.position = Vector3.Lerp(transform.root.position, targetPos, lerpTimer);
    }

    private void OnDisable()
    {
        if (LighthouseManager.Instance != null)
            LighthouseManager.Instance.onRadiusChange -= ApplyRadius;
    }
}
