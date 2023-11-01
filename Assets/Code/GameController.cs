using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
}
