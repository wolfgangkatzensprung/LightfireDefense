using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterfliesHandler : MonoBehaviour
{
    Butterfly[] butterflies = new Butterfly[5];
    Vector3 spawnPoint = new Vector3(0f, 1.4f, .25f);

    public LayerMask groundLayer;

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            butterflies[i] = transform.GetChild(i).GetComponent<Butterfly>();
        }

        StartCoroutine(DirectionChangeRoutine());
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        for (int i = 0; i < butterflies.Length; i++)
        {
            butterflies[i].Fly();
        }
    }

    internal void DoGetEaten(Butterfly b)
    {
        StartCoroutine(DinnerTimeRoutine(b));
    }

    IEnumerator DinnerTimeRoutine(Butterfly b)
    {
        b.isBeingEaten = true;
        b.FadeInShadow();
        b.direction = Vector3.zero;
        yield return new WaitForSeconds(1);
        b.shadowParticleSystem.transform.parent.SetParent(transform);
        b.gameObject.SetActive(false);
        yield return new WaitForSeconds(1);
        b.FadeOutShadow();
        StartCoroutine(RespawnRoutine(b));
    }

    IEnumerator RespawnRoutine(Butterfly b)
    {
        yield return new WaitForSeconds(Random.Range(3, 9));
        b.transform.position = spawnPoint;
        b.gameObject.SetActive(true);
        b.shadowParticleSystem.transform.parent.SetParent(b.transform);
        b.isBeingEaten = false;
    }

    IEnumerator DirectionChangeRoutine()
    {
        while (true)
        {
            ButterflyDirectionChange(butterflies[0]);
            yield return new WaitForSeconds(1);
            ButterflyDirectionChange(butterflies[1]);
            yield return new WaitForSeconds(1);
            ButterflyDirectionChange(butterflies[2]);
            yield return new WaitForSeconds(1);
            ButterflyDirectionChange(butterflies[3]);
            yield return new WaitForSeconds(1);
            ButterflyDirectionChange(butterflies[4]);
            yield return new WaitForSeconds(1);
        }
    }

    private void ButterflyDirectionChange(Butterfly b)
    {
        if (Vector3.Distance(b.transform.position, Vector3.zero) > LighthouseManager.Instance?.lighthouseRange && !b.isBeingEaten)
        {
            DoGetEaten(b);
            return;
        }

        Vector3 dir = Vector3.zero;

        if (Physics.Raycast(b.transform.position - new Vector3(0, 3f, 0), Vector3.down, out RaycastHit hit, 500f, groundLayer))
        {
            //Debug.Log($"Butterfly {b.gameObject} hit {hit.transform.name} at {hit.point}");
            dir = Random.insideUnitSphere;
        }
        else
        {
            dir = Vector3.Max(Random.insideUnitSphere, new Vector3(-1f, 1f, -1f));
        }

        b.direction = dir;
        b.transform.LookAt(dir - b.transform.position);
    }
}