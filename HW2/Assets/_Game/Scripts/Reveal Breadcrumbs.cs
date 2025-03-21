using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevealBreadcrumbs : MonoBehaviour
{
    MeshRenderer mr;

    void Start()
    {
        mr = GetComponent<MeshRenderer>();
        mr.enabled = false;
    }

    IEnumerator OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StaticVariables.bCaught = false;
            yield return new WaitForSeconds(2f);
            if (!StaticVariables.bCaught)
            {
                mr.enabled = true;
            }
        }
    }
}
