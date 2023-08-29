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
                    other.GetComponent<HealthComponent>().TakeDamage(baseDamage, false);
                }
                Destroy(gameObject);
            }
            else if (other.CompareTag("DestroyableObject"))
            {
                if (other.GetComponent<DestroyableObject>() != null)
                {
                    other.GetComponent<DestroyableObject>().DestroyObject();
                }
                Destroy(gameObject);
            }
            else if (other.CompareTag("ExplosiveObject"))
            {
                if (other.GetComponent<ExplosiveObject>() != null)
                {
                    other.GetComponent<ExplosiveObject>().TriggerExplosive();
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
