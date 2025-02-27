using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathParticles : MonoBehaviour
{
    [Header("References")]

    [Tooltip("Waypoint Path")]
    public Enemy_TD.WaypointPath firstPath = Enemy_TD.WaypointPath.WayR;
    [Tooltip("Waypoint Path starting at Wave 5")]
    public Enemy_TD.WaypointPath secondPath = Enemy_TD.WaypointPath.WayL;    
    [Tooltip("Waypoint Path for flying enemies, starting at Wave 10")]
    public Enemy_TD.WaypointPath flyingPath = Enemy_TD.WaypointPath.Direct;

    public Transform holderR;
    public Transform holderL;
    public Transform holderD;

    Transform[] firstWaypoints;
    Transform[] secondWaypoints;
    Transform[] flyingWaypoints;

    public TrailRenderer trailR;
    public ParticleSystem pathParticlesR;  
    
    public TrailRenderer trailL;
    public ParticleSystem pathParticlesL; 
    
    public TrailRenderer trailD;
    public ParticleSystem pathParticlesD;

    public float speed = 5f;
    [Tooltip("When distance to waypoint is lower than this value, next waypoint is targeted")]
    public float wayPointRange = 10f;

    Transform targetR;
    Transform targetL;
    Transform targetD;

    int waypointIndexR = 0;
    int waypointIndexL = 0;
    int waypointIndexD = 0;

    private void Start()
    {
        AssignWaypoints();
        holderR.position = targetR.position;
        holderL.position = targetL.position;
        holderD.position = targetD.position;

        ClearAllTrails();
    }

    private void ClearAllTrails()
    {
        ClearTrail(trailR);
        ClearTrail(trailL);
        ClearTrail(trailD);
    }

    private void Update()
    {
        if (EnemyWaveSpawner.Instance.isTDlevel && !EnemyWaveSpawner.Instance.isInWave)
        {
            MovePaths();
            DoWaypointCheck();

            if (!pathParticlesR.isPlaying)
                pathParticlesR.Play();   
            if (!pathParticlesL.isPlaying && EnemyWaveSpawner.Instance.GetWaveIndex() > 4)
                pathParticlesL.Play();      
            if (!pathParticlesD.isPlaying && EnemyWaveSpawner.Instance.GetWaveIndex() > 9)
                pathParticlesD.Play();
        }
        else if (pathParticlesR.isPlaying)
        {
            ClearTrail(trailR);
            pathParticlesR.Stop();
        }    
        else if (pathParticlesL.isPlaying)
        {
            ClearTrail(trailL);
            pathParticlesL.Stop();
        }  
        else if (pathParticlesD.isPlaying)
        {
            ClearTrail(trailD);
            pathParticlesD.Stop();
        }
    }

    void MovePaths()
    {
        Vector3 dirR = targetR.position - holderR.position;
        Vector3 dirL = targetL.position - holderL.position;
        Vector3 dirD = targetD.position - holderD.position;

        holderR.Translate(dirR.normalized * speed);
        holderL.Translate(dirL.normalized * speed);
        holderD.Translate(dirD.normalized * speed);
    }

    private void ClearTrail(TrailRenderer trail)
    {
        trail.Clear();
    }

    private void AssignWaypoints()
    {
        firstWaypoints = WayPoints.rPoints;
        secondWaypoints = WayPoints.lPoints;
        flyingWaypoints = WayPoints.dPoints;

        targetR = firstWaypoints[0];
        targetL = secondWaypoints[0];
        targetD = flyingWaypoints[0];
    }

    private void DoWaypointCheck()
    {
        float distanceToWaypointR = Vector3.Distance(holderR.position, targetR.position);
        float distanceToWaypointL = Vector3.Distance(holderL.position, targetL.position);
        float distanceToWaypointD = Vector3.Distance(holderD.position, targetD.position);

        if (distanceToWaypointR <= wayPointRange)
        {
            NextWayPointR();
        }   
        if (EnemyWaveSpawner.Instance.GetWaveIndex() > 4 && distanceToWaypointL <= wayPointRange)
        {
            NextWayPointL();
        }    
        if (EnemyWaveSpawner.Instance.GetWaveIndex() > 9 && distanceToWaypointD <= wayPointRange)
        {
            NextWayPointD();
        }
    }
    private void NextWayPointR()
    {
        if (waypointIndexR >= firstWaypoints.Length - 1)
        {
            waypointIndexR = 0;
            targetR = firstWaypoints[waypointIndexR];
            holderR.position = targetR.position;
            ClearTrail(trailR);
        }
        else
        {
            waypointIndexR++;
            targetR = firstWaypoints[waypointIndexR];
        }
    }  
    private void NextWayPointL()
    {
        if (waypointIndexL >= secondWaypoints.Length - 1)
        {
            waypointIndexL = 0;
            targetL = secondWaypoints[waypointIndexL];
            holderL.position = targetL.position;
            ClearTrail(trailL);
        }
        else
        {
            waypointIndexL++;
            targetL = secondWaypoints[waypointIndexL];
        }
    }    private void NextWayPointD()
    {
        if (waypointIndexD >= flyingWaypoints.Length - 1)
        {
            waypointIndexD = 0;
            targetD = flyingWaypoints[waypointIndexD];
            holderD.position = targetD.position;
            ClearTrail(trailD);
        }
        else
        {
            waypointIndexD++;
            targetD = flyingWaypoints[waypointIndexD];
        }
    }
}
