using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegenerationPickup : MonoBehaviour
{
    [SerializeField] Health health;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            health.RegenerationPickup();
            Destroy(gameObject);
            return;
        }
    }
}
