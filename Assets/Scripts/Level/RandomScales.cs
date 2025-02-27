using UnityEngine;

public class RandomScales : MonoBehaviour
{
    public Vector3 minScale = new Vector3(0.5f, 0.5f, 0.5f);
    public Vector3 maxScale = new Vector3(1.5f, 1.5f, 1.5f);

    private void Start()
    {
        foreach (Transform child in transform)
        {
            float rndScale = Random.Range(minScale.x, maxScale.x);
            Vector3 scale = Vector3.one * rndScale;
            child.localScale = scale;
        }
    }
}
