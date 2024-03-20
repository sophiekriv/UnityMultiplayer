using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Spawner : NetworkBehaviour
{
    // event that gets invoked when flower collected
    public event Action<RespawningFlower> Collected;

    // Variables
    [SerializeField] private RespawningFlower flowerPrefab; // reference to flower prefab
    [SerializeField] private int maxFlowers = 50; // max # of flowers to spawn
    [SerializeField] private int flowerValue = 10; // value of each flower
    [SerializeField] private Vector2 xSpawnRange; // range of x coordinate to spawn flowers
    [SerializeField] private Vector2 ySpawnRange; // range of y coordinates
    [SerializeField] private LayerMask layerMask; // layer mask of flower spawning

    // buffer for collider hits when flowers spawn
    private Collider2D[] flowerBuffer = new Collider2D[1];
    private float flowerRadius;
    public override void OnNetworkSpawn() {
        if (!IsServer) {
            return;
        }

        // get radius of flower prefab's collider
        flowerRadius = flowerPrefab.GetComponent<CircleCollider2D>().radius;

        // spawn max number of flwoers
        for (int i = 0; i < maxFlowers; i++) {
            SpawnFlower();
        }

    }

    // spawn flower at random location
    private void SpawnFlower() {
        // instantiate flower instance
        RespawningFlower flowerInstance = Instantiate(
            flowerPrefab, 
            GetSpawnPoint(), 
            Quaternion.identity);

        // set value of flower and spawn it in
        flowerInstance.SetValue(flowerValue);
        flowerInstance.GetComponent<NetworkObject>().Spawn();
        flowerInstance.Collected += HandleFlowerCollected;
    }
    
    // handle when flower is collected
    private void HandleFlowerCollected(RespawningFlower flower) {
        // respawn flower at random location
        flower.transform.position = GetSpawnPoint();
        flower.Reset();
    }

    // get random spawn point for a flower that is not colliding with anything 
    private Vector2 GetSpawnPoint() {
        float x = 0;
        float y = 0;
        while (true) {
            x = UnityEngine.Random.Range(xSpawnRange.x, xSpawnRange.y);
            y = UnityEngine.Random.Range(ySpawnRange.x, ySpawnRange.y);
            Vector2 spawnPoint = new Vector2(x, y);
            int numColliders = Physics2D.OverlapCircleNonAlloc(spawnPoint, flowerRadius, flowerBuffer, layerMask);
            if (numColliders == 0) {
                return spawnPoint;
            } 
        }
    }
}