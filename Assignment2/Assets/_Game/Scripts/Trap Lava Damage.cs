using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapLavaDamage : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private float trapDamage = 0f;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            health.Damage(trapDamage);
        }
    }
}
