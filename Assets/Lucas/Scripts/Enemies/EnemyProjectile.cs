using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField]
    private LayerMask hitLayerMask;
    [SerializeField]
    private int baseDamage;


    private void OnTriggerEnter(Collider other)
    {
        if ((hitLayerMask.value & 1 << other.gameObject.layer) == 1 << other.gameObject.layer)
        {

            if (other.CompareTag("Player"))
            {
                // Damage? 
                if (other.GetComponent<HealthComponent>() != null)
                {
                    other.GetComponent<HealthComponent>().TakeDamage(baseDamage);
                }
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
