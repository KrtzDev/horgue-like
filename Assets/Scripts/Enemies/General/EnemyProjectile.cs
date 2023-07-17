using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField]
    private LayerMask hitLayerMask;
    public int baseDamage;

    private void Start()
    {
        Destroy(gameObject,10f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((hitLayerMask.value & (1 << other.gameObject.layer)) > 0)
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
