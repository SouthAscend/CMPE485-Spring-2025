using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyLava : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StaticVariables.ObtainedLavaKey();
            Destroy(gameObject);
            return;
        }
    }
}
