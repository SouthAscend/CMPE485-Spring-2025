using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] string type = "Test Pickup";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PickupInventory.NewPickup(type);
            Destroy(gameObject);
            return;
        }
    }
}
