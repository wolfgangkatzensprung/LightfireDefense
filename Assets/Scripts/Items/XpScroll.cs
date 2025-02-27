using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class XpScroll : MonoBehaviour
{
    public int minXpValue = 25;
    public int maxXpValue = 500;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            int randomElement = Random.Range(1, 5);
            int randomXpAmount = Random.Range(minXpValue, maxXpValue) * PlayerExp.Instance.level[randomElement];
            PlayerExp.Instance.AddExp(randomXpAmount, randomElement);
            Destroy(gameObject);
        }
    }
}
