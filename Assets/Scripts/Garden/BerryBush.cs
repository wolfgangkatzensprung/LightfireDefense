using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerryBush : MonoBehaviour
{
    public Animator anim;

    public GameObject[] berries;
    public Transform harvestSpawnPos;
    [Tooltip("Ernteertrag Prefab zB Muenze")]
    public GameObject harvestPrefab;

    [Tooltip("Delay time between individual berries to grow")]
    public float growDelay = .1f;

    [Tooltip("Speed for grown Berries to be shot out of the bush")]
    public float berrySpeed = .1f;

    Vector3 startScale = new Vector3();
    float maxScale = .5f;
    internal float currentScale;

    internal int bushIndex = 0;

    private void OnEnable()
    {
        bushIndex = GardenManager.Instance.AddBerryBush(this);
    }

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();

        startScale = berries[0].transform.localScale;

        GardenManager.Instance.SaveBush(this);
    }

    private void ResetBerry(int i)
    {
        berries[i].transform.localScale = startScale * .1f;
        currentScale = berries[i].transform.localScale.x;
    }

    public void GrowBerries()
    {
        if (anim == null)
            anim = GetComponentInChildren<Animator>();

        anim.Play("Grow");

        StartCoroutine(GrowBerriesRoutine());
    }

    IEnumerator GrowBerriesRoutine()
    {
        for (int i = 0; i < berries.Length; i++)
        {
            currentScale = berries[i].transform.localScale.x;
            float scaleAdd = .2f;
            currentScale = Mathf.Min(currentScale + scaleAdd, maxScale);

            berries[i].transform.localScale = Vector3.Max(Vector3.one * currentScale, startScale);

            if (berries[i].transform.localScale.x >= maxScale)
            {
                if (Random.value > .5f)
                {
                    continue;
                }
                ProduceHarvest(i);
                yield return new WaitForSeconds(growDelay);
            }
            yield return new WaitForSeconds(growDelay);
        }

        GardenManager.Instance.SaveBush(this);
    }

    internal void LoadBerryScales(int i)
    {
        foreach (GameObject berry in berries)
        {
            berry.transform.localScale = Vector3.one * PlayerPrefs.GetFloat($"Bush{i}BerryScale");
        }
    }

    private void ProduceHarvest(int i)
    {
        GardenManager.Instance.PlaySuckParticles();
        ResetBerry(i);
        GameObject harvest = Instantiate(harvestPrefab, berries[i].transform.position, Quaternion.identity);
        GardenManager.Instance.AddBerry(harvest);

        float rndX = GlobalInfo.Instance.luckySpriteTrans.position.x;
        float rndY = GlobalInfo.Instance.luckySpriteTrans.position.z;
        Vector3 dir = (new Vector3(Mathf.PerlinNoise(rndX, rndY), 1f, Mathf.PerlinNoise(rndY, rndX)) - harvestSpawnPos.position).normalized;
        if (harvest.TryGetComponent(out Rigidbody rb))
        {
            rb.AddForce(dir * 5f + (Vector3.up * berrySpeed), ForceMode.Impulse);
        }
    }

    private void OnDisable()
    {
#if UNITY_EDITOR
        return;
#endif
        GardenManager.berryBushes.Remove(this);
    }
}