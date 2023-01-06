using System;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public event Action<float> OnHealthPercentChanged = delegate { };

    [field: SerializeField] public int MaxHealth { get; set; } = 100;
    [field: SerializeField] public int CurrentHealth { get; set; }

    private void Awake()
    {
        CurrentHealth = MaxHealth;
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;

        float currentHealthPct = (float)CurrentHealth / (float)MaxHealth;
        OnHealthPercentChanged(currentHealthPct);

        if (CurrentHealth <= 0)
        {
            if (this.gameObject.CompareTag("Player"))
            {
                Debug.Log("isDead");

                GameManager.Instance.PlayerDied();
            }

            if (this.gameObject.CompareTag("Enemy"))
            {
                Debug.Log("Enemy Died");

                GameManager.Instance.EnemyDied();

                this.gameObject.SetActive(false);
            }
        }
    }
}
