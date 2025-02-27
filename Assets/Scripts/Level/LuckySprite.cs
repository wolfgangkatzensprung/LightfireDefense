using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LuckySprite : MonoBehaviour
{
    Rigidbody rb;
    Collider col;

    [Header("LuckySprite")]
    [Tooltip("Orange Sprite => true ; Blue Sprite => false")]
    public bool isOrange = true;
    [Tooltip("FirePoint Transform for LineRenderer")]
    public Transform firePoint;
    [Tooltip("LineRenderer for BerryDance")]
    public LineRenderer lr;
    [Tooltip("Pick up Berry Range")]
    public float pickupRange = 2.5f;

    [Header("Movement")]
    [Tooltip("Horizontal Speed used in AddForce. Doesnt use normalized direction for cooler movement, so it needs to be very low.")]
    public float idleSpeed = .1f;
    [Tooltip("Horizontal Speed used in AddForce.")]
    public float moveToBerryForce = 12f;
    [Tooltip("Horizontal Speed used for MovePosition")]
    public float moveToBerrySpeed = 12f;
    [Tooltip("Speed of y amplitude modulation")]
    public float modulationSpeed = 1f;
    [Tooltip("Speed of Y modulation for random position")]
    public float ySpeed = 1f;

    [Tooltip("Height at which to orbit around Beacon")]
    public float orbitHeight = 23f;
    [Tooltip("Maximum and minimum value for random y position")]
    public float maxAmplitudeForRandomPos = 12f;
    [Tooltip("Maximum and minimum value for y modulation in movement")]
    public float maxAmplitudeForModulation = 12f;

    [Header("Rotation")]
    [Tooltip("Look Rotation Speed")]
    public float rotationSpeed = 1f;
    [Tooltip("Propability of applying torque when changing targetPos")]
    public float torqueApplyChance = .5f;
    [Tooltip("Torque that might randomly be applied")]
    public float torqueStrength = 9f;
    bool spinning = true;  // if LuckySprite is rotating around moving direction in DoMoveRotation()

    [Header("Timer")]
    float moveTimer = 0f;   // used in all moving modes
    [Tooltip("Average time until next target position change")]
    public float moveDelay = 3f;
    float rndTimerOffset;
    internal float beaconRange
    {
        get
        {
            if (LighthouseManager.Instance != null)
            {
                return LighthouseManager.Instance.lighthouseRange;
            }
            else { return 25f; }
        }
        set { }
    }
    [Header("Pickup and Follow Berry")]
    public float minGroundDistance = 5f;
    public LayerMask groundLayer;
    Transform target;
    Vector3 targetPos = new Vector3();
    Vector3 rndTorqueOffset = new Vector3();
    internal static bool foundBerry;    // current berry has been found and is bouncing around
    private Vector3 smoothVelocity;

    internal enum MoveMode
    {
        Idle,   // fliegt herum
        SearchBerry,   // sucht Berry und schnappt sie sich. mode geht bis zum Pickup aka StartBerryDance()
        BerryDance,   // folgt herumhopsender Berry
    }
    internal MoveMode mode = MoveMode.Idle;

    private void OnDisable()
    {
#if UNITY_EDITOR
        return;
#endif
        GardenManager.Instance.onGardenGrown -= DoBerryCheck;
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        if (GlobalInfo.Instance != null)
            GlobalInfo.Instance.luckySpriteTrans = transform;

        rndTimerOffset = Random.Range(-2f, 2f);

        if (GardenManager.Instance != null)
            GardenManager.Instance.onGardenGrown += DoBerryCheck;
    }

    void FixedUpdate()
    {
        if (mode == MoveMode.Idle)
            IdleMovement();
        else if (mode == MoveMode.SearchBerry)
            SearchBerryMovement();
        else if (mode == MoveMode.BerryDance)
        {
            BerryDanceMovement();
            PlayConnectionLine();
        }
    }

    private void DoBerryCheck()
    {
        Debug.Log("DoBerryCheck()");
        if (GardenManager.Instance.currentTargetBerryTrans != null)
            target = GardenManager.Instance.currentTargetBerryTrans;
        else
            target = null;

        if (mode != MoveMode.BerryDance)
        {
            if (target != null)
            {
                mode = MoveMode.SearchBerry;
            }
            else
            {
                mode = MoveMode.Idle;
            }
        }
    }

    private void IdleMovement()
    {
        moveTimer += Time.fixedDeltaTime;
        if (moveTimer > moveDelay + rndTimerOffset)
        {
            TargetPosChange();
            TargetRotChange();
            rndTimerOffset = Random.Range(-moveDelay * .333f, moveDelay * .333f);
            moveTimer = 0f;
        }
        DoIdleMove();
    }
    private void DoIdleMove()
    {
        Vector3 yModulationOffset = Vector3.up * ((Mathf.Sin(Time.time * modulationSpeed) + 1) * maxAmplitudeForModulation);
        Vector3 dir = targetPos - transform.position;
        rb.AddForce(dir * idleSpeed + yModulationOffset, ForceMode.Acceleration);

        DoMoveRotation();
    }

    private void DoMoveRotation()
    {
        Quaternion lookRot = Quaternion.LookRotation(rb.velocity);
        Quaternion targetRot = Quaternion.RotateTowards(transform.rotation, lookRot, Time.fixedDeltaTime * rotationSpeed);

        //Quaternion offsetLookRot = Quaternion.LookRotation(Vector3.Cross(rb.velocity, transform.up));
        //Quaternion offsetRot = Quaternion.RotateTowards(transform.rotation, offsetLookRot, Time.fixedDeltaTime * rotationSpeed);

        Quaternion deltaRotation = targetRot;
        rb.MoveRotation(deltaRotation);


        // Section for Video Movements

        // uebermuetige Sprites:
        //if (spinning)
        //{
        //    Vector3 torque = torqueStrength * transform.up;
        //    rb.AddRelativeTorque(torque, ForceMode.Acceleration);
        //}

        // sieht echt witzig aus wie sie rumwiggeln:
        //if (Random.value < torqueApplyChance) 
        //{
        //    Vector3 torque = torqueStrength * Vector3.Cross(rb.velocity, transform.forward);
        //    rb.AddRelativeTorque(torque, ForceMode.Acceleration);
        //}


    }
    private void DoMoveRotationDancing()
    {
        Quaternion lookRot = Quaternion.LookRotation(rb.velocity);
        Quaternion targetRot = Quaternion.RotateTowards(transform.rotation, lookRot, Time.fixedDeltaTime * rotationSpeed);

        if (Vector3.Distance(transform.position, target.position) < 3f)
        {
            transform.LookAt(target, Vector3.up);
            col.enabled = true;
        }
        else
        {
            if (col.enabled)
                col.enabled = false;

            Quaternion deltaRotation = targetRot;
            rb.MoveRotation(deltaRotation);
        }
    }

    private void TargetPosChange()
    {
        targetPos = GetRndTargetPosition();
        //Debug.Log($"targetPos: {targetPos}");
    }
    private void TargetRotChange()
    {
        rndTorqueOffset = Random.insideUnitSphere;
    }

    private Vector3 GetRndTargetPosition()
    {
        float x = Random.Range(-beaconRange, beaconRange);
        float z = Random.Range(-beaconRange, beaconRange);

        float sineY = Mathf.Sin(Time.time * ySpeed) * maxAmplitudeForRandomPos;
        float pingPong = (Mathf.PingPong(Time.time * .3f, 1f) + 1) * .5f;
        float perlin = Mathf.PerlinNoise(transform.position.x + pingPong, transform.position.z + pingPong);

        float y = sineY * perlin + orbitHeight;

        return new Vector3(x, y, z);
    }
    private void SearchBerryMovement()
    {
        if (GardenManager.Instance.currentTargetBerryTrans == null)
            return;

        moveTimer += Time.fixedDeltaTime;
        if (moveTimer > moveDelay + rndTimerOffset)
        {
            TargetRotChange();
            rndTimerOffset = Random.Range(-moveDelay * .333f, moveDelay * .333f);
            moveTimer = 0f;
        }
        Debug.Log($"targetPos: {target.position} und transformPosition {transform.position}, Distance: {Vector3.Distance(targetPos, transform.position)}");

        MoveTowardsBerry();
    }
    private void MoveTowardsBerry()
    {
        //Debug.Log($"MoveTowardsBerry() mit targetPos: {targetTrans.position} und transformPosition {transform.position}");

        Vector3 dir = (target.position - transform.position).normalized;

        float yDirection;

        if (Vector3.Distance(target.position, transform.position) > pickupRange * 3 && transform.position.y > 10f)
        {
            yDirection = Mathf.Lerp(-1f, 1, 1 / Mathf.Max(transform.position.y, 0.00001f));
            dir = new Vector3(dir.x, yDirection, dir.z);

            rb.AddForce(dir * moveToBerryForce, ForceMode.Acceleration);

            DoMoveRotation();
        }
        else
        {
            if (!rb.isKinematic)
            {
                rb.isKinematic = true;
            }

            rb.MovePosition(transform.position + dir * moveToBerrySpeed * Time.fixedDeltaTime);
            DoMoveRotation();

            if (Vector3.Distance(transform.position, target.position) < pickupRange)
            {
                if (target.TryGetComponent(out Berry berry) && !foundBerry)
                {
                    foundBerry = true;
                    rb.velocity = Vector3.zero;
                    StartBerryDance(berry);
                }
                else
                {
                    ResetModeToIdle();
                }
            }
        }
    }

    internal static void FinishBerryDance() 
    {
        foundBerry = false;
    }
    internal void ResetModeToIdle()
    {
        rb.velocity = Vector3.zero;
        rb.isKinematic = false;
        targetPos = Vector3.up * 55f;
        moveTimer = 0f;
        mode = MoveMode.Idle;
    }

    private void StartBerryDance(Berry berry)
    {
        Debug.Log("Pick Up Berry");

        Transform berryTrans = GardenManager.Instance.currentTargetBerryTrans;
        berry.StartRisingDance(this);
        
        rb.isKinematic = true;
        moveTimer = 0f;
        mode = MoveMode.BerryDance;
    }

    private void BerryDanceMovement()   // follow the berry around in the berry dance
    {
        moveTimer += Time.fixedDeltaTime;

        float x = Mathf.Cos(moveTimer);
        float y = Mathf.Sin(moveTimer + .25f);
        float z = Mathf.Sin(moveTimer);
        Vector3 sphericalOffset = new Vector3(x, y, z);
        Vector3 direction = (target.position - transform.position).normalized;

        Vector3 nextPosition = Vector3.SmoothDamp(rb.position, target.position + sphericalOffset - direction * 3f, ref smoothVelocity, Time.fixedDeltaTime, moveToBerrySpeed);

        RaycastHit groundHit;
        if (Physics.Raycast(transform.position, Vector3.down, out groundHit, minGroundDistance, groundLayer))
        {
            float distanceToGround = groundHit.distance;
            if (distanceToGround < minGroundDistance)
            {
                nextPosition.y = groundHit.point.y + minGroundDistance;
            }
        }

        rb.MovePosition(nextPosition);
        DoMoveRotationDancing();
    }

    private void PlayConnectionLine()   // magical connection between berry and luckysprite
    {
        lr.SetPosition(0, firePoint.position);
        lr.SetPosition(1, target.position);
    }
    internal void ResetConnectionLine()
    {
        lr.SetPositions(new Vector3[] {
            firePoint.position,
            firePoint.position
        });
    }

    internal void HappyRotation()
    {
        rb.AddRelativeTorque(Vector3.right * 3f, ForceMode.Impulse);
    }
}