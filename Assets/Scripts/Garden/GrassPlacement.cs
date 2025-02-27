using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassPlacement : MonoBehaviour
{
    public GameObject terrain;
    Mesh terrainMesh;

    public GameObject[] grassPrefabs;

    private void Awake()
    {
        terrainMesh = terrain.GetComponent<MeshFilter>().sharedMesh;
    }

    private void OnEnable()
    {
        LighthouseManager.Instance.onRadiusChange += UpdateGrass;
    }

    private void UpdateGrass()
    {
        // x = LighthouseManager.Instance.lighthouseRange 

        for (int i = 0; i < terrainMesh.vertices. Length; i++)
        {
            Debug.Log($"{i}: {terrainMesh.vertices[i].ToString()}");
        }

    }

    private void OnDisable()
    {
#if UNITY_EDITOR
        LighthouseManager.Instance.onRadiusChange -= UpdateGrass;
#endif
    }
}
