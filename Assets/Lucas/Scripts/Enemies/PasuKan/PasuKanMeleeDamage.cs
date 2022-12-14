using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasuKanMeleeDamage : MonoBehaviour
{
    private bool didDamage = false;

    private void OnEnable()
    {
        didDamage = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<HealthComponent>() != null)
            {
                if(!didDamage)
                {
                    other.GetComponent<HealthComponent>().TakeDamage(gameObject.GetComponentInParent<Enemy>().EnemyData._basicDamage);
                    didDamage = true;
                }
            }
        }
    }
}
