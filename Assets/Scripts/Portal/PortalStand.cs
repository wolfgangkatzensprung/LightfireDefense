using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PortalStand : MonoBehaviour
{
    public GameObject portal;
    public TextMeshProUGUI displayText;
    [Tooltip("Particles to be spawned upon putting the pieces together")]
    public GameObject particlesPrefab;

    public Animator portalPiecesAnimator;

    int pieces = 0;

    private void Start()
    {
        if (PlayerPrefs.GetInt("PortalReady") > 0)
        {
            gameObject.SetActive(false);
        }
        else if (PlayerPrefs.HasKey("PortalPieces"))
        {
            pieces = PlayerPrefs.GetInt("PortalPieces");
        }

        UpdateText();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PortalPiece piece))
        {
            DealWithIt(piece);
        }
    }

    private void DealWithIt(PortalPiece piece)
    {
        if (pieces >= 3)
            return;

        SpawnParticles(piece.transform.position);
        SpawnParticles(transform.position + Vector3.up * 3f);
        Destroy(piece.gameObject);
        pieces += 1;
        UpdateText();
        PlayerPrefs.SetInt("PortalPieces", pieces);

        if (pieces > 2)
        {
            StartCoroutine(PutPiecesTogether());
            PlayerPrefs.SetInt("PortalReady", 1);
            SaveSystem.SaveGame();
        }
    }

    private void SpawnParticles(Vector3 pos)
    {
        Instantiate(particlesPrefab, pos, Quaternion.identity);
    }

    private void UpdateText()
    {
        displayText.text = $"{pieces}/3";
    }

    IEnumerator PutPiecesTogether()
    {
        portalPiecesAnimator.Play("PutPiecesTogether");
        yield return new WaitForSeconds(portalPiecesAnimator.GetCurrentAnimatorStateInfo(0).length);
        portal.SetActive(true);
    }
}
