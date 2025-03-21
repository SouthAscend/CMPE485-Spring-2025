using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevealPath : MonoBehaviour
{
    MeshRenderer mr = null;
    BoxCollider bc = null;

    private void Start()
    {
        Transform parent = transform.parent;
        mr = parent.GetComponent<MeshRenderer>();
        bc = parent.GetComponent<BoxCollider>();
        mr.enabled = false;
        bc.enabled = false;
        if (!mr || !bc) Debug.Log("BBBB");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!mr || !bc) Debug.Log("AAAA");
            mr.enabled = true;
            bc.enabled = true;
        }
    }
}
