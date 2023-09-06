using UnityEngine;

public class Spikes : MonoBehaviour
{
    public int _trapDamage;

    private void OnTriggerEnter(Collider other)
    {
        // Allowing different trap damage scaling
        if (other.CompareTag("Enemy"))
        {
            if(other.GetComponent<AI_Agent_Enemy>()._isBossEnemy)
            {
                if (other.GetComponent<HealthComponent>() != null)
                {
                    other.GetComponent<HealthComponent>().TakeDamage(_trapDamage = (int)(other.GetComponent<HealthComponent>().maxHealth * 0.025f), false);
                }
            }
            else
            {
                if (other.GetComponent<HealthComponent>() != null)
                {
                    other.GetComponent<HealthComponent>().TakeDamage(_trapDamage = other.GetComponent<HealthComponent>().maxHealth, false);
                }
            }
        }
        else if (other.CompareTag("Player"))
        {
            if (other.GetComponent<HealthComponent>() != null)
            {
                other.GetComponent<HealthComponent>().TakeDamage(_trapDamage, false);
            }
        }
    }
}