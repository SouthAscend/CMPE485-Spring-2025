using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleKeyDrop : MonoBehaviour
{
    [SerializeField] PlayerController pc;

    void Update()
    {
        if (transform.position.y < -2)
        {
            pc.Lost();
        }
    }
}
