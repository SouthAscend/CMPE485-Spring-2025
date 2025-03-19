using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyIce : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("IceKey"))
        {
            StaticVariables.PushedIceKey();
            Destroy(other.gameObject);
            Destroy(gameObject);
            return;
        }
    }
}
