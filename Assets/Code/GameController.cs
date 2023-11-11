using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour
{

    public static GameController instance;

    // Outlet
    public Transform[] spawnPoints;
    public GameObject zombiePrefab;
    public GameObject ammunitionPrefab;
    public GameObject medKitPrefab;
    public TMP_Text textTimer;

    // State Tracking
    public float timeElapsed;
    public bool timeGoing;
    private TimeSpan timePlaying;
    private int maxScores = 3; // Number of high scores to track each level

    void Awake()
    {
        instance = this;
        StartCoroutine("ZombieSpawnTimer");
        StartCoroutine("AmmunitionSpawnTimer");
        StartCoroutine("MedKitSpawnTimer");
    }

    void Start()
    {
        timeGoing = true;
        timeElapsed = 0f;
        textTimer.text = "Time: 00:00.00";
    }

    void Update()
    {
        if(timeGoing)
        {
            // Increment passage of time for each frame of the game
            timeElapsed += Time.deltaTime;

            // Update Timer Counter
            timePlaying = TimeSpan.FromSeconds(timeElapsed);
            textTimer.text = "Time: " + timePlaying.ToString("mm':'ss'.'ff");
        }
    }

    void SpawnZombie()
    {
        // Spawn
        int randomSpawnIndex = UnityEngine.Random.Range(0, spawnPoints.Length);
        Transform randomSpawnPoint = spawnPoints[randomSpawnIndex];
        Instantiate(zombiePrefab, randomSpawnPoint.position, Quaternion.identity);
    }

    IEnumerator ZombieSpawnTimer()
    {
        // Wait
        yield return new WaitForSeconds(5);

        // Spawn Zombie
        SpawnZombie();

        // Repeat
        StartCoroutine("ZombieSpawnTimer");
    }

    void SpawnAmmunition()
    {
        Vector3 distance = new Vector3(UnityEngine.Random.Range(-15, 0), 0, 0);
        // Spawn
        int randomSpawnIndex = UnityEngine.Random.Range(0, spawnPoints.Length);
        Transform randomSpawnPoint = spawnPoints[randomSpawnIndex];
        Instantiate(ammunitionPrefab, randomSpawnPoint.position + distance, Quaternion.identity);
    }

    IEnumerator AmmunitionSpawnTimer()
    {
        // Wait
        yield return new WaitForSeconds(10);

        // Spawn Ammunition
        SpawnAmmunition();

        // Repeat
        StartCoroutine("AmmunitionSpawnTimer");
    }

    void SpawnMedKit()
    {
        Vector3 distance = new Vector3(UnityEngine.Random.Range(-15, 0), 0, 0);
        // Spawn
        int randomSpawnIndex = UnityEngine.Random.Range(0, spawnPoints.Length);
        Transform randomSpawnPoint = spawnPoints[randomSpawnIndex];
        Instantiate(medKitPrefab, randomSpawnPoint.position + distance, Quaternion.identity);
    }

    IEnumerator MedKitSpawnTimer()
    {
        // Wait
        yield return new WaitForSeconds(20);

        // Spawn Ammunition
        SpawnMedKit();

        // Repeat
        StartCoroutine("MedKitSpawnTimer");
    }


    // Stop timer & track current score at the end of the level
    public void CompleteRound()
    {
        // Stop Timer
        timeGoing = false;

        // Get Current Level
        int currentLevel = SceneManager.GetActiveScene().buildIndex - 1;

        // Get scores from memory and add new score
        List<string> scores = GetScores(currentLevel);

        scores.Add(timeElapsed.ToString());

        // Sort ascending, ignoring default 0 scores
        scores.Sort((a, b) =>
        {
            if (a == "0" && b == "0")
            {
                return 0; // Both are 0, no change in order
            }
            else if (a == "0")
            {
                return 1; // x is 0, so y comes before x
            }
            else if (b == "0")
            {
                return -1; // y is 0, so x comes before y
            }
            return float.Parse(a).CompareTo(float.Parse(b));

        });

        // Keep only top maxScores scores
        if (scores.Count > maxScores)
        {
            scores.RemoveRange(maxScores, scores.Count - maxScores);
        }

        SaveScores(scores, currentLevel);
    }

    // Retrieve scores from PlayerPrefs
    private List<string> GetScores(int level)
    {
        // Save Default scores
        if (!PlayerPrefs.HasKey("Level_" + level))
        {
            List<string> initialScores = new List<string>();
            for (int i = 0; i < maxScores; i++)
            {
                initialScores.Add("0");
            }
            SaveScores(initialScores, level);
        }
        string scoreString = PlayerPrefs.GetString("Level_" + level);
        return new List<string>(scoreString.Split(','));
    }

    // Save scores to PlayerPrefs
    private void SaveScores(List<string> scores, int level)
    {
        string scoreString = string.Join(",", scores);
        PlayerPrefs.SetString("Level_" + level, scoreString);
        PlayerPrefs.Save();
    }
}
