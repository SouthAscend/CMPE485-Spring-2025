using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField] private GameObject prefab;  // Will be manually assigned from the Inspector
    private Vector3 lastSpawnedPosition; // Storing the last positions to prevent collision clashes
    [SerializeField] private bool canSpawn = false; // New prefabs won't be able to spawn new ones, this will be overriden for the original instance

    void Start()
    {
        lastSpawnedPosition = transform.position;
    }

    void Update()
    {
        if (canSpawn && Input.GetKeyDown(KeyCode.Space))  // Spawns on "Space"
        {
            SpawnPrefab();
        }
    }

    private void SpawnPrefab()
    {
        GameObject newObject = Instantiate(prefab, lastSpawnedPosition + Vector3.up * 1, Quaternion.identity);
        lastSpawnedPosition = newObject.transform.position; // Updates the last spawn position
    }
}
