using System;
using UnityEngine;
using UnityEngine.AI;

public class HealthComponent : MonoBehaviour
{
	public event Action<float> OnHealthPercentChanged;
	public event Action OnDeath;

	[SerializeField] public int _maxHealth;
	[SerializeField] public int _currentHealth;

	public bool _isDead = false;

	protected virtual void Awake()
	{
		_isDead = false;
	}

	public virtual void TakeDamage(int damage)
	{
		_currentHealth -= damage;

		float currentHealthPct = (float)_currentHealth / _maxHealth;
		OnHealthPercentChanged?.Invoke(currentHealthPct);

		if (gameObject.CompareTag("Player"))
		{
			GameObject.FindGameObjectWithTag("UI")?.GetComponentInChildren<UIDamageFlash>().DamageFlash(0.25f, .5f);

			if (_currentHealth <= 0 && !_isDead)
			{
				Debug.Log("GotDamage");
				GameManager.Instance.PlayerDied();
				_isDead = true;
			}
		}
		else
		{
			if(_currentHealth <= 0 && !_isDead)
			{
				OnDeath?.Invoke();
				_isDead = true;
			}
		}
	}
}
