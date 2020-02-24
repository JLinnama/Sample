using System.Collections;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    // Controls the gameplay loop

    public static GameManager instance;

    public GameObject player;
    public Camera cam;

    public static bool gameOn;

    public GameObject menuView;
    public GameObject ingameView;

    float nextMissileSpawn;

    public static int incomingMissiles;
    public static int missilesDeployed;
    public static int missilesDestroyed;

    int currentRound;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI hiscoreText;

    public static int score;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        DataManager.instance.WriteSaveString(true, null, 0, DataManager.aircraftSave, CollectionManager.instance.planeWorldScroller);

        CollectionManager.instance.UpdatePlaneSelection();
        UpdateHiscore();
    }

    private void Update()
    {
        cam.transform.position = Vector3.Lerp(cam.transform.position, player.transform.position, 20f * Time.deltaTime);
        cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, -10.0f);

        if (gameOn)
        {
            nextMissileSpawn -= 1.0f * Time.deltaTime;

            if (nextMissileSpawn <= 0.0f)
            {
                if (missilesDeployed < incomingMissiles)
                {
                    MissilePool.instance.SpawnMissile();
                    nextMissileSpawn = Random.Range(2.0f, 4.0f);

                    missilesDeployed += 1;
                }
            }
        }
    }

    public void StartGame()
    {
        if (DataManager.instance.IsItemUnlocked(DataManager.aircraftSave, CollectionManager.selectedPlane))
        {
            gameOn = true;

            menuView.GetComponent<Animator>().SetInteger("State", 1);
            ingameView.GetComponent<Animator>().SetInteger("State", 1);

            nextMissileSpawn = 2.0f;
            currentRound = 0;
            score = 0;

            OnRoundStart();
            PlayerController.instance.OnStartGame();
            UpdateScore();
        }
    }

    void OnRoundStart()
    {
        currentRound += 1;

        incomingMissiles = currentRound * 2;
        missilesDeployed = 0;
        missilesDestroyed = 0;
    }

    public void FinishRound()
    {
        StartCoroutine(StartNewRoundWithDelay());
    }

    IEnumerator StartNewRoundWithDelay()
    {
        yield return new WaitForSeconds(2.0f);

        OnRoundStart();
    }

    public IEnumerator EndGame()
    {
        gameOn = false;

        MissilePool.instance.OnGameEnd();

        ingameView.GetComponent<Animator>().SetInteger("State", 0);

        yield return new WaitForSeconds(1.0f);

        menuView.GetComponent<Animator>().SetInteger("State", 0);

        if (score > PlayerPrefs.GetInt (DataManager.hiscoreSave))
        {
            DataManager.instance.Save(DataManager.hiscoreSave, score);
            UpdateHiscore();
        }

        foreach (int requirement in CollectionManager.instance.aircraftScoreRequirements)
        {
            if (score >= requirement)
            {
                DataManager.instance.WriteSaveString(false, "b", CollectionManager.instance.aircraftScoreRequirements.IndexOf (requirement), DataManager.aircraftSave, CollectionManager.instance.planeWorldScroller);
            }
        }

        PlayerController.instance.OnReturnToMainMenu();

        CollectionManager.instance.UpdatePlaneSelection();

    }

    public void UpdateScore()
    {
        DataManager.instance.UpdateText(scoreText, score.ToString());
        scoreText.GetComponent<Animation>().Play();
    }

    public void UpdateHiscore()
    {
        DataManager.instance.UpdateText(hiscoreText, PlayerPrefs.GetInt(DataManager.hiscoreSave).ToString());
    }
}
