using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShadowEnemy))]
public class ShadowCollisions : MonoBehaviour
{
    ShadowEnemy shadow;
    ShadowEnemiesHandler shadowHandler;

    private void Start()
    {
        shadow = GetComponent<ShadowEnemy>();
        shadowHandler = ShadowEnemiesHandler.Instance;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Shadow collided with " + collision.gameObject.name);
        if (collision.transform.CompareTag("LighthouseCollider"))
        {
            //Debug.Log("Shadow Ends Chase");
            Puff();
        }
    }

    public void Puff()
    {
        Debug.Log($"{gameObject.name} Puff");
        shadowHandler.EndChase(shadow);
        StartCoroutine(ShadowRespawnRoutine());
    }

    private IEnumerator ShadowRespawnRoutine()
    {
        yield return new WaitForSeconds(4f);
        if (!LightRadiusHandler.playerInside)
            shadowHandler.StartChase(shadow);
    }
}