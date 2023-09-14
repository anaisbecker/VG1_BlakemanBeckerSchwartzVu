using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public static GameController instance;

    // Outlet
    public Transform spawnPoint;
    public GameObject zombiePrefab;

    // State Tracking
    public float timeElapsed;

    void Awake()
    {
        instance = this;
        StartCoroutine("ZombieSpawnTimer");
    }


    void Update()
    {
        // Increment passage of time for each frame of the game
        timeElapsed += Time.deltaTime;
    }

    void SpawnZombie()
    {
        // Spawn 
        Instantiate(zombiePrefab, spawnPoint.position, Quaternion.identity);
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
}
