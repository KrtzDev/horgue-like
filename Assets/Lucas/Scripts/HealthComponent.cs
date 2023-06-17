using System;
using UnityEngine;
using UnityEngine.AI;

public class HealthComponent : MonoBehaviour
{
	public event Action<float> OnHealthPercentChanged;
	public event Action OnDeath;

	[field: SerializeField] public int MaxHealth { get; set; } = 100;
	[field: SerializeField] public int CurrentHealth { get; set; }

	[field: SerializeField] public Animator Animator { get; private set; }
	[field: SerializeField] public Enemy Enemy { get; private set; }

	[field: SerializeField] private Transform _hitParticlePosition;
	[field: SerializeField] private ParticleSystem _hitParticle;

	private bool _isDead = false;

	[Header("Enemy Drops")]
	[Range(0, 100)]
	public int dropChance;
	public GameObject healthDrop;
	public GameObject coinDrop;

	private void Awake()
	{
		if (gameObject.CompareTag("Enemy"))
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

		ParticleSystem HitParticle = Instantiate(_hitParticle, _hitParticlePosition.transform.position, Quaternion.identity, transform);
		HitParticle.Play();

		float currentHealthPct = (float)CurrentHealth / MaxHealth;
		OnHealthPercentChanged?.Invoke(currentHealthPct);

		if (gameObject.CompareTag("Player"))
		{
			GameObject.FindGameObjectWithTag("UI")?.GetComponentInChildren<UIDamageFlash>().DamageFlash(0.25f, .5f);

			if (CurrentHealth <= 0 && !_isDead)
			{
				GameManager.Instance.PlayerDied();
				_isDead = true;

				OnDeath?.Invoke();
			}
		}
		else if (gameObject.CompareTag("Enemy"))
		{
			if (CurrentHealth <= 0 && !_isDead)
			{
				GameManager.Instance.EnemyDied();

				MarkEnemyToDie();

				Animator.SetTrigger("death");
				_isDead = true;

				OnDeath?.Invoke();
			}
			else if (CurrentHealth > 0 && !_isDead)
			{
				Animator.SetTrigger("damage");
			}
		}
	}

	private void MarkEnemyToDie()
	{
		if (gameObject.GetComponent<Animator>() != null)
			gameObject.GetComponent<Animator>().SetBool("isDying", true);
		if (gameObject.GetComponent<Collider>() != null)
			gameObject.GetComponent<Collider>().enabled = false;
		if (gameObject.GetComponent<Rigidbody>() != null)
			gameObject.GetComponent<Rigidbody>().isKinematic = true;
		if (gameObject.GetComponent<NavMeshAgent>() != null)
			gameObject.GetComponent<NavMeshAgent>().enabled = false;
		if (gameObject.GetComponent<Enemy>() != null)
			gameObject.GetComponent<Enemy>().enabled = false;

		gameObject.tag = "Untagged";
	}

	private void DropScore()
	{
		Instantiate(coinDrop, _hitParticlePosition.position, Quaternion.identity);
		GameManager.Instance._currentScore += Enemy.EnemyData._givenXP;
	}

	private void DropHealthPotion()
	{
		int random = UnityEngine.Random.Range(0, 100);
		if (random <= dropChance)
		{
			Instantiate(healthDrop, _hitParticlePosition.position, Quaternion.identity);
		}
	}
}
