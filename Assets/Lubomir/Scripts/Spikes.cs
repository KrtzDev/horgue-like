using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField]
    private int trapDamage = 10;

    private void OnTriggerEnter(Collider other)
    {
        // Allowing different trap damage scaling
        if (other.CompareTag("Enemy"))
        {
            if (other.GetComponent<HealthComponent>() != null)
            {
                other.GetComponent<HealthComponent>().TakeDamage(trapDamage);
            }
        }
        else if (other.CompareTag("Player"))
        {
            if (other.GetComponent<HealthComponent>() != null)
            {
                other.GetComponent<HealthComponent>().TakeDamage(trapDamage);
            }
        }
    }
}