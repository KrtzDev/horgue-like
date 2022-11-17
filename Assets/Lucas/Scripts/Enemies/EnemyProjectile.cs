using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField]
    private LayerMask hitLayerMask;

    private void OnTriggerEnter(Collider other)
    {
        if ((hitLayerMask.value & 1 << other.gameObject.layer) == 1 << other.gameObject.layer)
        {

            if (other.CompareTag("Player"))
            {
                Destroy(gameObject);
                // Damage?
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

}
