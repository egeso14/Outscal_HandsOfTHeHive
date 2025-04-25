using System;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject beePrefab;
    
    private int maxBeesInScene;
    private Vector3 beeSpawnPosition;
    private Timer spawnTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DeterminePosition();
        maxBeesInScene = LevelConfig.instance.maxBees;
        spawnTimer = new Timer(LevelConfig.instance.beeSpawnInterval);
        beeSpawnPosition = LevelConfig.instance.beeSpawnLocation;
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

        transform.position = beeSpawnPoint + hiveDistanceToSpawn
                            + new Vector3(0, bounds.extents.y, 0);
    }

    private void MaybeSpawn()
    {
        if (BeeBrain.numberOfActiveBees >= maxBeesInScene)
        {
            return;
        }
        spawnTimer.TickTimer();
        if (spawnTimer.Check())
        {
            Instantiate(beePrefab, beeSpawnPosition, Quaternion.identity);
        }
    }
}
