using System;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public event Action<float> OnHealthPercentChanged;

    [field: SerializeField] public int MaxHealth { get; set; } = 100;
    [field: SerializeField] public int CurrentHealth { get; set; }

    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public Enemy Enemy { get; private set; }

    private bool _isDead = false;

    private void Awake()
    {
        if (this.gameObject.CompareTag("Enemy"))
        {
            Enemy = gameObject.GetComponent<Enemy>();
            MaxHealth = Enemy.EnemyData._maxHealth;
        }
       
        CurrentHealth = MaxHealth;
        _isDead = false;
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;

        float currentHealthPct = (float)CurrentHealth / (float)MaxHealth;
        OnHealthPercentChanged?.Invoke(currentHealthPct);

        if (this.gameObject.CompareTag("Enemy"))
        {
            if(CurrentHealth > 0)
            {
                Animator.SetTrigger("damage");
            }
        }
        

        if (this.gameObject.CompareTag("Player"))
        {
            GameObject.FindGameObjectWithTag("UI").GetComponentInChildren<UIDamageFlash>().DamageFlash(0.25f, .5f);
        }

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
                GameManager.Instance._currentScore += Enemy.EnemyData._givenXP;

                this.Animator.SetTrigger("death");
            }

            _isDead = true;
        }
    }
}
