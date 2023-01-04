using System;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public event Action<float> OnHealthPercentChanged = delegate { };

    [field: SerializeField] public int MaxHealth { get; set; } = 100;
    [field: SerializeField] public int CurrentHealth { get; set; }

    EnemyAgent agent;

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
            }

            if (this.gameObject.CompareTag("Enemy"))
            {
                Debug.Log("Enemy Died");

                agent = GetComponent<EnemyAgent>();

                EnemyDeathState deathState = agent.stateMachine.GetState(EnemyStateID.Death) as EnemyDeathState;
                agent.stateMachine.ChangeState(EnemyStateID.Death);

                GameManager.Instance.EnemyDied();
            }
        }
    }
}
