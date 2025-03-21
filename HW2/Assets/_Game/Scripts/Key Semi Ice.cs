using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySemiIce : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StaticVariables.ice_semi_key = true;
            Destroy(gameObject);
            return;
        }
    }
}
