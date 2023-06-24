using System;
using UnityEngine;
using UnityEngine.AI;

public class HealthComponent : MonoBehaviour
{
	public event Action<float> OnHealthPercentChanged;
	public event Action OnDeath;

	[SerializeField] public int _maxHealth;
	[SerializeField] public int _currentHealth;

	public Animator _anim;
	public Enemy _enemy;

	[SerializeField] private Transform _hitParticlePosition;
	[SerializeField] private ParticleSystem _hitParticle;

	public bool _isDead = false;

	[Header("Enemy Drops")]
	private int _healthDropChance;
	public GameObject _healthDrop;
	public GameObject _coinDrop;

	private void Awake()
	{
		if (gameObject.CompareTag("Enemy"))
		{
			_enemy = gameObject.GetComponent<Enemy>();
			_anim = gameObject.GetComponent<Animator>();
			_maxHealth = _enemy.EnemyData._maxHealth;
			_currentHealth = _maxHealth;
			_healthDropChance = (int)(_enemy.EnemyData._healthDropChance * 100);
		}

		_isDead = false;
	}

	public void TakeDamage(int damage)
	{
		_currentHealth -= damage;

		ParticleSystem HitParticle = Instantiate(_hitParticle, _hitParticlePosition.transform.position, Quaternion.identity, transform);
		HitParticle.Play();

		float currentHealthPct = (float)_currentHealth / _maxHealth;
		OnHealthPercentChanged?.Invoke(currentHealthPct);

		if (gameObject.CompareTag("Player"))
		{
			GameObject.FindGameObjectWithTag("UI")?.GetComponentInChildren<UIDamageFlash>().DamageFlash(0.25f, .5f);

			if (_currentHealth <= 0 && !_isDead)
			{
				GameManager.Instance.PlayerDied();
				_isDead = true;

				OnDeath?.Invoke();
			}
		}
		else if (gameObject.CompareTag("Enemy"))
		{
			if (_currentHealth <= 0 && !_isDead)
			{
				GameManager.Instance.EnemyDied();

				MarkEnemyToDie();

				_anim.SetTrigger("death");
				_isDead = true;

				OnDeath?.Invoke();
			}
			else if (_currentHealth > 0 && !_isDead)
			{
				_anim.SetTrigger("damage");
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
		Instantiate(_coinDrop, _hitParticlePosition.position, Quaternion.identity);
		GameManager.Instance._currentScore += _enemy.EnemyData._givenXP;
	}

	private void DropHealthPotion()
	{
		int random = UnityEngine.Random.Range(0, 100);
		if (random <= _healthDropChance)
		{
			Instantiate(_healthDrop, _hitParticlePosition.position, Quaternion.identity);
		}
	}
}
