using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MoneyCoin : MonoBehaviour
{
    public int value = 25;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            PlayerMoney.Instance.AddMoney(value);
            Destroy(gameObject);
        }
    }
}
