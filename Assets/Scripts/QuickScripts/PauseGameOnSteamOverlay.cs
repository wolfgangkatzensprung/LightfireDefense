using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class PauseGameOnSteamOverlay : MonoBehaviour
{
	protected Callback<GameOverlayActivated_t> m_GameOverlayActivated;

	private void OnEnable()
	{
		if (SteamManager.Initialized)
		{
			m_GameOverlayActivated = Callback<GameOverlayActivated_t>.Create(OnGameOverlayActivated);
		}
	}
	private void OnGameOverlayActivated(GameOverlayActivated_t pCallback)
    {
        if (pCallback.m_bActive != 0)
        {
            GameController.Instance.PauseGame();
        }
        else
        {
            UIManager.Instance.TryHideEscapeMenu();
            UIManager.Instance.TryHideUpgradeMenu();
            GameController.Instance.UnpauseGame();
            UIManager.Instance.MenuToggle(false);
        }
    }
}