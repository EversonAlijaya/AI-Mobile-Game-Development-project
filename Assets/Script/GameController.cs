using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class GameController : MonoBehaviour
{
    public GameObject[] hazards;
    public Vector3 spawnValues;
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;
    public GameObject bossPrefab;
    public int bossScoreThreshold = 100;
    private bool bossTriggered;


    public TMP_Text scoreText;
    public TMP_Text restartText;
    public TMP_Text gameOverText;
    public TMP_Text bossWarningText;

    private bool gameOver;
    private bool restart;
    private int score;

    private void Start()
    {
        gameOver = false;
        restart = false;
        restartText.text = "";
        gameOverText.text = "";
        bossWarningText.text = "";
        score = 0;
        UpdateScore();
        StartCoroutine(SpawnWaves());
    }

    private void Update()
    {
        if (restart)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    private IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        while (true)
        {
            for (int i = 0; i < hazardCount; i++)
            {
                GameObject hazard = hazards[Random.Range(0, hazards.Length)];
                Vector3 spawnPosition = new Vector3
                (
                    Random.Range(-spawnValues.x, spawnValues.x),
                    spawnValues.y,
                    spawnValues.z
                );
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(hazard, spawnPosition, spawnRotation);
                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds(waveWait);

            spawnWait = Mathf.Max(0.1f, spawnWait - 0.1f);
            waveWait = Mathf.Max(1f, waveWait - 0.3f);

            if (score >= bossScoreThreshold && !bossTriggered)
            {
                bossTriggered = true;
                StartCoroutine(BossSequence());
                break;
            }

            if (gameOver)
            {
                restartText.text = "Press 'R' for Restart";
                restart = true;
                break;
            }
        }
    }
    private IEnumerator BossSequence()
    {
        bossWarningText.text = "BOSS INCOMING";
        yield return new WaitForSeconds(3f);
        bossWarningText.text = "";

        Vector3 bossSpawnPosition = new Vector3(0, spawnValues.y, spawnValues.z);
        Instantiate(bossPrefab, bossSpawnPosition, Quaternion.identity);
    }
    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
    }

    private void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    public void GameOver()
    {
        gameOverText.text = "Game Over!";
        gameOver = true;
    }
}
