using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyHealth))]
public class Enemy_ParticlesOnDamaged : MonoBehaviour
{
    public GameObject damagedParticlePrefab;
    public GameObject deathParticlePrefab;

    private void Start()
    {
        EnemyHealth eh = GetComponent<EnemyHealth>();
        eh.onDeath += OnEnemyDeath;
        eh.onDamaged += OnEnemyDamaged;
    }

    private void OnEnemyDeath()
    {
        if (enabled)
            Instantiate(deathParticlePrefab, transform.position, Quaternion.identity);
    }
    void OnEnemyDamaged(Damage.DamageType dmgType)
    {
        Instantiate(damagedParticlePrefab, transform.position, Quaternion.identity);
    }

    private void OnDisable()
    {
#if UNITY_EDITOR
        return;
#endif
        GetComponent<EnemyHealth>().onDeath -= OnEnemyDeath;
    }
}
