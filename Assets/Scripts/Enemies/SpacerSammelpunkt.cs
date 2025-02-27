using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpacerSammelpunkt : MonoBehaviour
{
    internal static Vector3 sammelPunkt = new Vector3(0, 100, 0);

    public static List<Transform> spacers = new List<Transform>();

    static internal List<Transform> unassignedStuckMobs = new List<Transform>();

    const int maxColliders = 15;
    float minRange = 10f;
    const float addRange = 10f;

    private void Update()
    {
        Collider[] colliders = new Collider[maxColliders];
        int colliderAmount = Physics.OverlapSphereNonAlloc(transform.position, minRange, colliders);
        for (int i = 0; i < colliderAmount; i++)
        {
            if (colliders[i].TryGetComponent(out Spacer spacerComponent))
            {
                if (colliders[i].transform.childCount > 0)
                {
                    Debug.Log($"{spacerComponent.name} state: Waypoint");
                    spacerComponent.state = Spacer.SpacerState.Waypoint;
                }
                else
                {
                    if (TryAssignNextStuckMobTo(spacerComponent))
                    {
                        Debug.Log($"{spacerComponent.name} state: Pickup");
                        spacerComponent.GotoPickupState();
                    }
                    else
                    {
                        Debug.Log($"{spacerComponent.name} state: Return");
                        spacerComponent.GotoReturnState();
                    }
                }

            }
        }

        //minRange += Mathf.PingPong(Time.time, addRange);
    }

    internal bool TryAssignNextStuckMobTo(Spacer spacer)
    {
        if (spacer.assignedMobTrans != null)
            return true;


        if (unassignedStuckMobs.Count > 0)
        {
            Transform[] unassignedMobs = unassignedStuckMobs.ToArray();
            if (unassignedMobs.Length > 0)
            {
                spacer.assignedMobTrans = unassignedMobs[0];
                unassignedStuckMobs.Remove(unassignedMobs[0]);
            }
            return true;
        }
        return false;
    }
}