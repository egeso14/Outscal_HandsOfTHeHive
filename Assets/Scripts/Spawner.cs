using System;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject beePrefab;
    [SerializeField] private int beesToSpawn;
    private int beesSpawned;

    private int maxBeesInScene;

    private Timer spawnTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //DeterminePosition();
        maxBeesInScene = LevelConfig.instance.maxBees;
        spawnTimer = new Timer(LevelConfig.instance.beeSpawnInterval);
        beesSpawned = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MaybeSpawn();
    }

    private void DeterminePosition()
    {
        var beeSpawnPoint = LevelConfig.instance.beeSpawnLocation;
        Vector3 hiveDistanceToSpawn = LevelConfig.instance.hiveDistanceToSpawnPoint;
        var renderer = GetComponentInChildren<Renderer>();
        var bounds = renderer.bounds;

    }

    private void MaybeSpawn()
    {
        if (beesSpawned >= beesToSpawn)
        {
            return;
        }
        spawnTimer.TickTimer();
        if (spawnTimer.Check())
        {
            Instantiate(beePrefab, transform.position, Quaternion.identity);
            beesSpawned++;
        }
    }
}
