using System;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public event Action<float> OnHealthPctChanged = delegate { };

    [SerializeField]
    private int maxHealth = 100;
    [SerializeField]
    private int currentHealth;


    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        float currentHealthPct = (float)currentHealth / (float)maxHealth;
        OnHealthPctChanged(currentHealthPct);

        if (currentHealth <= 0)
        {
            Debug.Log("isDead");

            if (this.gameObject.CompareTag("Enemy"))
            {
                Destroy(this.gameObject);
            }
        }
    }
}
