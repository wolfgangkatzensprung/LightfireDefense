using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovementController))]
public class PlayerDash : MonoBehaviour
{
    PlayerMovementController pmc;
    Rigidbody rb;

    bool canDash = true;
    bool dash;

    [Tooltip("Impulse Speed applied to Rigidbody")]
    public float dashSpeed = 77f;

    [Tooltip("Delay until Player can move again after Dash")]
    public float dashMoveDelay = .12f;

    [Tooltip("Time until Player can dash again (Cooldown Time)")]
    public float dashRepeatDelay = 1.8f;

    // Multiplier is based on how far Player is from a wall. 0 = touching wall ; 1 = distance to wall is raycastDistance or higher
    float dashSpeedMultiplier = 1f;
    RaycastHit hit;
    float raycastDistance = 7f;

    public LayerMask groundLayer;

    public delegate void DashDelegate();
    public DashDelegate onDash;

    private void Start()
    {
        pmc = GetComponent<PlayerMovementController>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (canDash && !GlobalInfo.inMenu && Input.GetButtonDown("Dash"))
        {
            dash = true;
        }
    }

    private void FixedUpdate()
    {
        if (dash)
        {
            dash = false;
            Dash();
        }
    }

    void Dash()
    {
        StartCoroutine(DashingRoutine());
        onDash?.Invoke();
    }

    IEnumerator DashingRoutine()
    {
        canDash = false;
        pmc.isDashing = true;

        SoundManager.Instance.PlaySoundAt(SoundManager.Sound.PlayerDash, transform.position);

        Vector3 dir = transform.forward;
        if (Input.GetKey(KeyCode.S))
            dir = -transform.forward;

        dashSpeedMultiplier = GetDashSpeedMultiplierByRaycast(dir);
        float finalDashSpeed = dashSpeed * dashSpeedMultiplier;
        rb.AddForce(dir * finalDashSpeed, ForceMode.Impulse);
        yield return new WaitForSeconds(dashMoveDelay);

        pmc.isDashing = false;
        yield return new WaitForSeconds(dashRepeatDelay);

        canDash = true;
    }

    private float GetDashSpeedMultiplierByRaycast(Vector3 dir)
    {
        return 1f;

        float distance = 0f;
        float multiplier = 0f;

        Vector3 raycastStartPoint = GlobalInfo.Instance.mainCam.position;

        if (Physics.Raycast(raycastStartPoint, dir, out hit, raycastDistance, groundLayer))
        {
            Debug.DrawRay(raycastStartPoint, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            distance = Vector3.Distance(raycastStartPoint, hit.point);
            Debug.Log($"Distance to wall: {distance}");
            multiplier = Mathf.Min(distance * .3f, 1f);
            multiplier = Mathf.Min(multiplier * .2f, 1f);
            Debug.Log($"DashSpeedMultiplier: {multiplier}");
        }
        else
        {
            multiplier = 1f;
        }

        return multiplier;
    }
}
