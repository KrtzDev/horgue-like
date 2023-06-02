using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDrop : MonoBehaviour
{
    public int HealthAmount;
    public LayerMask _groundLayer;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            HealthComponent hp = other.GetComponent<HealthComponent>();

            hp.CurrentHealth += HealthAmount;
            if(hp.CurrentHealth > hp.MaxHealth)
            {
                hp.CurrentHealth = hp.MaxHealth;
            }
            Destroy(transform.parent.gameObject);
        }
    }
}
