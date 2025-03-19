using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInvisible : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("InvisibleKey"))
        {
            StaticVariables.invisible_key = true;
            Destroy(other.gameObject);
            Destroy(gameObject);
            return;
        }
    }
}
