using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapIce : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] Health health;

    IEnumerator OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            yield return new WaitForSeconds(0.1f);
            float forceMagnitude = Random.Range(25f, 45f);
            Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
            rb.AddForce(randomDirection * forceMagnitude * 2f, ForceMode.Impulse);
            health.Damage(forceMagnitude * .33f);
        }
    }
}
