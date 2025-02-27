using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalPieceButton : MonoBehaviour
{
    public GameObject portalPiecePrefab;

    public void SpawnPortalPiece()
    {
        Instantiate(portalPiecePrefab, GlobalInfo.Instance.playerTrans.position, Quaternion.identity);
    }
}
