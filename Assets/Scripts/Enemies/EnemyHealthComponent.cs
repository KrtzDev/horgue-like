using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthComponent : HealthComponent
{
    [SerializeField] private Transform _hitParticlePosition;
    [SerializeField] private ParticleSystem _hitParticle;

	private AI_Agent _enemy;

    [Header("Enemy Drops")]
    private int _healthDropChance;
    public GameObject _healthDrop;
    public GameObject _coinDrop;

    protected override void Awake()
    {
        base.Awake();

        _enemy = gameObject.GetComponent<AI_Agent>();
        _maxHealth = _enemy._enemyData._maxHealth;
        _currentHealth = _maxHealth;
        _healthDropChance = (int)(_enemy._enemyData._healthDropChance * 100);
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

		if(_currentHealth > 0)
        {
			MarkEnemyToTakeDamage();
        }
		else if (_currentHealth <= 0)
        {
			MarkEnemyToDie();
        }
    }

    private void MarkEnemyToTakeDamage()
	{
		_enemy._animator.SetTrigger("damage");
	}

	private void MarkEnemyToDie()
	{
		if (gameObject.GetComponent<Animator>() != null)
			gameObject.GetComponent<Animator>().SetBool("isDying", true);
		if (gameObject.GetComponent<Collider>() != null)
			gameObject.GetComponent<Collider>().enabled = false;
		if (gameObject.GetComponent<Rigidbody>() != null)
			gameObject.GetComponent<Rigidbody>().isKinematic = true;
		if (gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>() != null)
			gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
		if (gameObject.GetComponent<AI_Agent>() != null)
			gameObject.GetComponent<AI_Agent>().enabled = false;

		GameManager.Instance.EnemyDied();

		_enemy._stateMachine.ChangeState(AI_StateID.Death);

		gameObject.tag = "Untagged";
	}

	private void DropScore()
	{
		Instantiate(_coinDrop, _hitParticlePosition.position, Quaternion.identity);
		GameManager.Instance._currentScore += _enemy._enemyData._givenXP;
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
