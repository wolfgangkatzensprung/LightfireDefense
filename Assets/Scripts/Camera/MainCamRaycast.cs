using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamRaycast : Singleton<MainCamRaycast>
{
    Ray ray;
    RaycastHit hit;
    RaycastHit lastHit;
    RaycastHit enemyHit;

    [Tooltip("Maximum Raycast Distance")]
    public float maxDistance = 100f;

    public bool aimingAtEnemy;
    public bool aimingAtAnything;

    internal Vector3 facingDirection;

    public LayerMask enemyLayer;
    public LayerMask defaultLayer;
    public LayerMask interactableLayer;
    public LayerMask groundLayer;
    LayerMask combinedLayers;

    private void Start()
    {
        combinedLayers = enemyLayer | defaultLayer | interactableLayer | groundLayer;
    }

    private void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        CombinedLayersRaycast();

        EnemyLayerRaycast();
    }

    private void EnemyLayerRaycast()
    {
        if (Physics.Raycast(ray, out enemyHit, maxDistance, enemyLayer, QueryTriggerInteraction.Ignore))
        {
            aimingAtEnemy = true;
            aimingAtAnything = true;
            
            Debug.Log($"MCR Enemy Hit: {enemyHit.collider.name} at {enemyHit.point}");
        }
        else
        {
            aimingAtEnemy = false;
        }

    }

    private void CombinedLayersRaycast()
    {
        if (Physics.Raycast(ray, out hit, maxDistance, combinedLayers, QueryTriggerInteraction.Ignore))
        {
            Debug.DrawRay(GlobalInfo.Instance.firePoint.position, hit.point);
            //Debug.DrawLine(GlobalInfo.Instance.firePoint.position, ray.direction);
            lastHit = hit;
            aimingAtAnything = true;
            
            //Debug.Log($"MCR Hit: {hit.collider.name} at {hit.point}");
        }
        else
        {
            aimingAtAnything = false;
        }

    }

    public Ray GetRay()
    {
        return ray;
    }

    public RaycastHit GetLastHit()
    {
        return lastHit;
    }

    public RaycastHit GetLastEnemyHit()
    {
        return enemyHit;
    }

}
