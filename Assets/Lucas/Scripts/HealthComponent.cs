using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class HealthComponent : MonoBehaviour
{
    public event Action<float> OnHealthPercentChanged;

    [field: SerializeField] public int MaxHealth { get; set; } = 100;
    [field: SerializeField] public int CurrentHealth { get; set; }

    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public Enemy Enemy { get; private set; }

    [field: SerializeField] private Transform _HitParticlePosition;
    [field: SerializeField] private ParticleSystem _HitParticle;

    private bool _isDead = false;

    [Header("Enemy Drops")]
    [Range(0, 100)]
    public int dropChance;
    public GameObject HealthDrop;
    public GameObject CoinDrop;

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

        ParticleSystem HitParticle = Instantiate(_HitParticle, _HitParticlePosition.transform.position, Quaternion.identity, this.transform);
        HitParticle.Play();

        float currentHealthPct = (float)CurrentHealth / (float)MaxHealth;
        OnHealthPercentChanged?.Invoke(currentHealthPct);        

        if (this.gameObject.CompareTag("Player"))
        {
            GameObject.FindGameObjectWithTag("UI")?.GetComponentInChildren<UIDamageFlash>().DamageFlash(0.25f, .5f);

            if(CurrentHealth <= 0 && !_isDead)
            {
                GameManager.Instance.PlayerDied();
                _isDead = true;
            }
        }
        else if (this.gameObject.CompareTag("Enemy"))
        {
            if (CurrentHealth <= 0 && !_isDead)
            {
                GameManager.Instance.EnemyDied();

                MarkEnemyToDie();

                this.Animator.SetTrigger("death");
                _isDead = true;
            }
            else if (CurrentHealth > 0 && !_isDead)
            {
                this.Animator.SetTrigger("damage");
            }
        }
    }

    private void MarkEnemyToDie()
    {
        if (this.gameObject.GetComponent<Animator>() != null)
        {
            this.gameObject.GetComponent<Animator>().SetBool("isDying", true);
        }

        if (this.gameObject.GetComponent<Collider>() != null)
        {
            this.gameObject.GetComponent<Collider>().enabled = false;
        }

        if (this.gameObject.GetComponent<Rigidbody>() != null)
        {
            this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }

        if(this.gameObject.GetComponent<NavMeshAgent>() != null)
        {
            this.gameObject.GetComponent<NavMeshAgent>().enabled = false;
        }

        if(this.gameObject.GetComponent<Enemy>() != null)
        {
            this.gameObject.GetComponent<Enemy>().enabled = false;
        }

        this.gameObject.tag = "Untagged";
    }

    private void DropScore()
    {
        Instantiate(CoinDrop, _HitParticlePosition.position, Quaternion.identity);
        GameManager.Instance._currentScore += Enemy.EnemyData._givenXP;
    }

    private void DropHealthPotion()
    {
        int random = UnityEngine.Random.Range(0, 100);
        if (random <= dropChance)
        {
            Instantiate(HealthDrop, _HitParticlePosition.position, Quaternion.identity);
        }
    }
}
