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

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            mr.enabled = true;
        }
    }
}
