using System.Collections.Generic;
using UnityEngine;

public class SpawnPickups : MonoBehaviour
{
    [SerializeField] private List<GameObject> pickupPrefabs = new List<GameObject>();
    [SerializeField] private List<float> probabilities = new List<float>();

    void Start()
    {
        NormalizeProbabilities();
        SpawnPickup();
        Destroy(gameObject);
    }

    private void NormalizeProbabilities()
    {
        float total = 0f;
        foreach (float prob in probabilities)
            total += prob;

        if (total == 0)
        {
            Debug.LogError("Total probability is zero! Assign proper values.");
            return;
        }

        for (int i = 0; i < probabilities.Count; i++)
            probabilities[i] /= total;
    }

    private void SpawnPickup()
    {
        if (pickupPrefabs.Count == 0 || probabilities.Count != pickupPrefabs.Count)
        {
            Debug.LogError("Pickup list is empty or mismatched with probabilities!");
            return;
        }

        float randomPoint = Random.value;
        float currentSum = 0f;

        for (int i = 0; i < pickupPrefabs.Count; i++)
        {
            currentSum += probabilities[i];
            if (randomPoint <= currentSum)
            {
                Instantiate(pickupPrefabs[i], new Vector3(transform.position.x, 1f, transform.position.z), Quaternion.identity);
                break;
            }
        }
    }
}
