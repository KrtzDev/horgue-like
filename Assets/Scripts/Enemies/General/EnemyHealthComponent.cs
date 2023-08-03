using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthComponent : HealthComponent
{
    [SerializeField] private Transform _hitParticlePosition;
    [SerializeField] private ParticleSystem _hitParticle;
	public EnemyHealthBar _enemyHealthBar;

	private AI_Agent_Enemy _enemy;

    [Header("Enemy Drops")]
    private int _healthDropChance;
    public GameObject _healthDrop;
    public GameObject _coinDrop;

    protected override void Awake()
    {
        base.Awake();

        _enemy = gameObject.GetComponent<AI_Agent_Enemy>();
        _maxHealth = _enemy._enemyData._maxHealth;
        _currentHealth = _maxHealth;
		_enemyHealthBar = gameObject.GetComponentInChildren<EnemyHealthBar>();
        _healthDropChance = (int)(_enemy._enemyData._healthDropChance * 100);
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

		ParticleSystem hitParticle = _hitParticle;
		Instantiate(hitParticle, _hitParticlePosition.position, Quaternion.identity);

		if (_currentHealth <= 0 && !_isDead && _canTakeDamage)
		{
			MarkEnemyToDie();
			return;
		}
		
		if (_currentHealth > 0 && !_isDead && !_enemy._isBossEnemy && _canTakeDamage)
        {
			MarkEnemyToTakeDamage();
        }

		if(_enemy._isBossEnemy && _canTakeDamage)
        {
			_enemy.CheckForBossStage();
        }
    }

    private void MarkEnemyToTakeDamage()
	{
		_enemy._rb.velocity = new Vector3(0, _enemy._rb.velocity.y, 0);
		_enemy._animator.SetTrigger("damage");
	}

	private void MarkEnemyToDie()
	{
		if (gameObject.GetComponent<Collider>() != null)
			gameObject.GetComponent<Collider>().enabled = false;
		if (gameObject.GetComponent<Rigidbody>() != null)
			gameObject.GetComponent<Rigidbody>().isKinematic = true;
		if (gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>() != null && !_enemy._isBossEnemy)
			gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
		if (gameObject.GetComponent<AI_Agent_Enemy>() != null)
			gameObject.GetComponent<AI_Agent_Enemy>().enabled = false;

		gameObject.tag = "Untagged";

		_isDead = true;

		if(_enemy._isBossEnemy)
        {
			GameManager.Instance._newGamePlus = true;
        }

		GameManager.Instance.EnemyDied();

		_enemy._stateMachine.ChangeState(AI_StateID.Death);
	}

	public void DropScore()
	{
		GameObject newCoin;
		newCoin = Instantiate(_coinDrop, _hitParticlePosition.position, Quaternion.identity);
		newCoin.GetComponentInChildren<CoinDrop>()._givenScore =_enemy._enemyData._givenXP;
	}

	public void DropHealthPotion()
	{
		int random = Random.Range(0, 100);
		if (random < _healthDropChance)
		{
			Vector3 spawnPos = _hitParticlePosition.position;

			Instantiate(_healthDrop, spawnPos, Quaternion.identity);
		}
	}

	public void SetAgentActive() => _enemy.enabled = true;
	public void CanTakeDamageActive() => _canTakeDamage = true;
}
