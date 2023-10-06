using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public static GameController instance;

    // Outlet
    public Transform[] spawnPoints;
    public GameObject zombiePrefab;
    public GameObject ammunitionPrefab;
    public GameObject medKitPrefab;

    // State Tracking
    public float timeElapsed;

    void Awake()
    {
        instance = this;
        StartCoroutine("ZombieSpawnTimer");
        StartCoroutine("AmmunitionSpawnTimer");
        StartCoroutine("MedKitSpawnTimer");
    }


    void Update()
    {
        // Increment passage of time for each frame of the game
        timeElapsed += Time.deltaTime;
    }

    void SpawnZombie()
    {
        // Spawn
        int randomSpawnIndex = Random.Range(0, spawnPoints.Length);
        Transform randomSpawnPoint = spawnPoints[randomSpawnIndex];
        Instantiate(zombiePrefab, randomSpawnPoint.position, Quaternion.identity);
    }

    IEnumerator ZombieSpawnTimer()
    {
        // Wait
        yield return new WaitForSeconds(10);

        // Spawn Zombie
        SpawnZombie();

        // Repeat
        StartCoroutine("ZombieSpawnTimer");
    }

    void SpawnAmmunition()
    {
        Vector3 distance = new Vector3(Random.Range(-15, 0), 0, 0);
        // Spawn
        int randomSpawnIndex = Random.Range(0, spawnPoints.Length);
        Transform randomSpawnPoint = spawnPoints[randomSpawnIndex];
        Instantiate(ammunitionPrefab, randomSpawnPoint.position + distance, Quaternion.identity);
    }

    IEnumerator AmmunitionSpawnTimer()
    {
        // Wait
        yield return new WaitForSeconds(15);

        // Spawn Ammunition
        SpawnAmmunition();

        // Repeat
        StartCoroutine("AmmunitionSpawnTimer");
    }

    void SpawnMedKit()
    {
        Vector3 distance = new Vector3(Random.Range(-15, 0), 0, 0);
        // Spawn
        int randomSpawnIndex = Random.Range(0, spawnPoints.Length);
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
