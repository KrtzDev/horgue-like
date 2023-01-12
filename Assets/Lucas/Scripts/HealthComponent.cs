using System;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public event Action<float> OnHealthPercentChanged = delegate { };

    [field: SerializeField] public int MaxHealth { get; set; } = 100;
    [field: SerializeField] public int CurrentHealth { get; set; }

    [field: SerializeField] public Animator Animator { get; private set; }

    private bool _isDead = false;

    private void Awake()
    {
        CurrentHealth = MaxHealth;
        _isDead = false;
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;

        float currentHealthPct = (float)CurrentHealth / (float)MaxHealth;
        OnHealthPercentChanged(currentHealthPct);

        /* if (this.gameObject.CompareTag("Enemy"))
        {
            if(CurrentHealth > 0)
            {
                Animator.SetTrigger("damage");
            }
        }
        */

        if (CurrentHealth <= 0 && !_isDead)
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

                this.Animator.SetTrigger("death");
            }

            _isDead = true;
        }
    }
}
