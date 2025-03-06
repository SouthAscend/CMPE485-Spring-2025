using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnteredLava : MonoBehaviour
{
    [SerializeField] Lava lavaComponent;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            lavaComponent.setActive(true);
        }
    }
}
