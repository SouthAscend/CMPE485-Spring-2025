using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceWall : MonoBehaviour
{
    private bool bActive = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && StaticVariables.ice_semi_key && !bActive)
        {
            bActive = true;
            StartCoroutine(DescendWall());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && StaticVariables.ice_semi_key && bActive)
        {
            bActive = false;
            StartCoroutine(AscendWall());
        }
    }

    IEnumerator DescendWall()
    {
        while (true)
        {
            if (bActive && transform.position.y > -2.2f)
            {
                transform.position = new Vector3(transform.position.x, Mathf.Max(-2.21f, transform.position.y - Time.deltaTime), transform.position.z);
            }
            if (!bActive) break;
            yield return null;
        }
    }

    IEnumerator AscendWall()
    {
        while (true)
        {
            if (!bActive && transform.position.y < 0f)
            {
                transform.position = new Vector3(transform.position.x, Mathf.Min(0.01f, transform.position.y + Time.deltaTime), transform.position.z);
            }
            if (bActive) break;
            yield return null;
        }
    }


}
