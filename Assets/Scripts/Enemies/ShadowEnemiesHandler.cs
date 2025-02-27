using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

// GameObject is disabled and will only be activated when Intro is done
public class ShadowEnemiesHandler : Singleton<ShadowEnemiesHandler>
{
    public ShadowEnemy[] shadows;
    bool chasing;

    float delay = 1f;       // time to wait until particles have stopped spawning, so shadows can be repositioned to spread posis

    private void OnEnable()
    {
        LighthouseManager.Instance.onRadiusChange += SpreadShadows;
    }
    private void OnDisable()
    {
        if (LighthouseManager.Instance != null)
            LighthouseManager.Instance.onRadiusChange -= SpreadShadows;
    }

    private void Start()
    {
        ActivateShadows();
        SpreadShadows();
    }

    private void ActivateShadows()
    {
        foreach (ShadowEnemy se in shadows)
        {
            se.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        if (!chasing && !LightRadiusHandler.playerInside)
        {
            StartChase();
        }
        else if (chasing && LightRadiusHandler.playerInside)
        {
            EndChase();
        }
    }

    private void StartChase()
    {
        for (int i = 0; i < shadows.Length; i++)
        {
            ShadowEnemy shadow = shadows[i];
            shadow.chasing = true;
            shadow.FadeIn();
        }

        chasing = true;
    }
    private void EndChase()
    {
        for (int i = 0; i < shadows.Length; i++)
        {
            ShadowEnemy shadow = shadows[i];
            shadow.chasing = false;
            shadow.FadeOut();
        }

        SpreadShadows();

        chasing = false;
    }

    internal void StartChase(ShadowEnemy shadow)
    {
        shadow.chasing = true;
        shadow.FadeIn();

        chasing = true;
    }
    internal void EndChase(ShadowEnemy shadow)
    {
        shadow.chasing = false;
        shadow.FadeOut();

        RelocateShadow(shadow);

        chasing = false;
    }
    void RelocateShadow(ShadowEnemy shadow)
    {
        float lhRange = LighthouseManager.Instance.lighthouseRange;

        Vector3 rnd = Random.insideUnitSphere * 100f;
        float rndX = 0f;
        float rndZ = 0f;

        if (rnd.x < 0)
            rndX = Mathf.Min(rnd.x, -lhRange - 4f);
        else if (rnd.x > 0)
            rndX = Mathf.Max(rnd.x, lhRange + 4f);

        if (rnd.z < 0)
            rndZ = Mathf.Min(rnd.z, -lhRange - 4f);
        else if (rnd.z > 0)
            rndZ = Mathf.Max(rnd.z, lhRange + 4f);

        shadow.transform.position = new Vector3(rndX, 4f, rndZ);
        //Debug.Log($"New Shadow position: {shadow.transform.position} (Lighthouse Range is {lhRange} and rnd was {rnd} ");
        shadow.ResetChasingTime();
    }
    void SpreadShadows()
    {
        float lhRange = LighthouseManager.Instance.lighthouseRange;

        for (int i = 0; i < shadows.Length; i++)
        {
            Vector3 rnd = Random.insideUnitSphere * 100f;
            float rndX = 0f;
            float rndZ = 0f;

            if (rnd.x < 0)
                rndX = Mathf.Min(rnd.x, -lhRange - 4f);
            else if (rnd.x > 0)
                rndX = Mathf.Max(rnd.x, lhRange + 4f);

            if (rnd.z < 0)
                rndZ = Mathf.Min(rnd.z, -lhRange - 4f);
            else if (rnd.z > 0)
                rndZ = Mathf.Max(rnd.z, lhRange + 4f);

            shadows[i].transform.position = new Vector3(rndX, 4f, rndZ);
            //Debug.Log($"New Shadow {i} position: {shadows[i].transform.position} (Lighthouse Range is {lhRange} and rnd was {rnd} ");
        }
    }
}