using UnityEngine;

public class IntroHandlerAndTrigger : MonoBehaviour
{
    Transform playerTrans;
    PlayerShooting ps;

    [Tooltip("References")]
    public Transform introHut;
    public GameObject introFinisherCanvas;
    public Transform introSpawnPoint;
    public Transform beaconSpawnPoint;
    public GameObject shadowEnemiesHandler;

    private void Start()
    {
        playerTrans = GlobalInfo.Instance.playerTrans;
        ps = playerTrans.GetComponent<PlayerShooting>();

        if (!GlobalInfo.isNewStart)
        {
            SkipIntro();
            return;
        }

        SpawnPlayerIntro();
    }

    private void SpawnPlayerIntro()
    {
        GlobalInfo.Instance.playerTrans.position = introSpawnPoint.position;
        //GlobalInfo.Instance.playerTrans.rotation = introSpawnPoint.rotation;
        GameController.Instance.SetPlayerPositionAndRotation(introSpawnPoint.position, introSpawnPoint.rotation);

        Debug.Log("Player Intro Spawn");

        ps.wandEquipped = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LaunchIntroSequence();
        }
    }

    private void LaunchIntroSequence()
    {
        Debug.Log("Launch Intro Sequence");

        MusicManager.Instance.PlayIdleMusic();

        introFinisherCanvas.SetActive(true);

        gameObject.SetActive(false);
    }

    public void SkipIntro()
    {
        Debug.Log("Skip Intro");

        GlobalInfo.Instance.playerRb.velocity = Vector3.zero;
        playerTrans.position = beaconSpawnPoint.position;

        GlobalInfo.Instance.magicWand.SetActive(true);
        ps.wandEquipped = true;
        shadowEnemiesHandler.SetActive(true);

        GameController.Instance.FinishIntro();

        introHut.gameObject.SetActive(false);
    }
}