using System;
using UnityEngine;
using UnityEngine.AI;

public class HealthComponent : MonoBehaviour
{
	public event Action<float> OnHealthPercentChanged;
	public event Action OnDeath;

	public int _maxHealth;
	public int _currentHealth;
	public bool _canTakeDamage;

	public bool _isDead = false;

	protected virtual void Awake()
	{
		_canTakeDamage = true;
		_isDead = false;
	}

	public virtual void TakeDamage(int damage)
	{
		if(_canTakeDamage)
        {
			_currentHealth -= damage;
			float currentHealthPct = (float)_currentHealth / _maxHealth;
			OnHealthPercentChanged?.Invoke(currentHealthPct);
		}

		if (gameObject.CompareTag("Player"))
		{
			FindObjectOfType<UIDamageFlash>().DamageFlash(0.25f, .4f, .2f);

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
			}
		}
	}
}
