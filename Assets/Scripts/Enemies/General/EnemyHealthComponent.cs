using System;
using UnityEngine;

public class EnemyHealthComponent : HealthComponent
{
	public event Action OnEnemyDied;

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

		_enemy = GetComponent<AI_Agent_Enemy>();

		_enemyHealthBar = gameObject.GetComponentInChildren<EnemyHealthBar>();
        _healthDropChance = (int)(_enemy._enemyData._healthDropChance * 100);
    }

    public override void TakeDamage(int damage, bool damageOverTime)
    {
        base.TakeDamage(damage, false);

		ParticleSystem hitParticle = _hitParticle;
		Instantiate(hitParticle, _hitParticlePosition.position, Quaternion.identity);

		if (currentHealth <= 0 && !isDead && canTakeDamage)
		{
			MarkEnemyToDie();
			return;
		}
		
		if (currentHealth > 0 && !isDead && !_enemy._isBossEnemy && canTakeDamage)
        {
			MarkEnemyToTakeDamage();
        }

		if(_enemy._isBossEnemy && canTakeDamage)
        {
			_enemy.CheckForBossStage();
        }
    }

    private void MarkEnemyToTakeDamage()
	{
		_enemy.RigidBody.velocity = new Vector3(0, _enemy.RigidBody.velocity.y, 0);
		_enemy.Animator.SetTrigger("damage");
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

		OnEnemyDied?.Invoke();

		gameObject.tag = "Untagged";

		isDead = true;

		if(_enemy._isBossEnemy)
        {
			AudioManager.Instance.PlaySound("RikayonScream");
			GameManager.Instance.killedBoss = true;
			GameManager.Instance._newGamePlus = true;
		}

		GameManager.Instance.EnemyDied();

		_enemy.StateMachine.ChangeState(AI_StateID.Death);
	}

	public void DropScore()
	{
		GameObject newCoin;
		newCoin = Instantiate(_coinDrop, _hitParticlePosition.position, Quaternion.identity);
		newCoin.GetComponentInChildren<Coin_Collectible>().givenScore =_enemy._enemyData._givenXP;
	}

	public void DropHealthPotion()
	{
		int random = UnityEngine.Random.Range(0, 100);
		if (random < _healthDropChance)
		{
			Vector3 spawnPos = _hitParticlePosition.position;

			Instantiate(_healthDrop, spawnPos, Quaternion.identity);
		}
	}

	public void CanTakeDamageActive() => canTakeDamage = true;


	public override void DamageFlash()
	{
		if (_isDamageFlashing)
			return;

		if (GetComponentInChildren<SkinnedMeshRenderer>() != null)
		{
			for (int i = 0; i < _skinnedMeshes.Length; i++)
			{
				_originalMaterials[i] = _skinnedMeshes[i].material;
				_skinnedMeshes[i].material = _damageFlashMaterial;
			}
		}
		else if (GetComponentInChildren<MeshRenderer>() != null)
		{
			for (int i = 0; i < _meshes.Length; i++)
			{
				_originalMaterials[i] = _meshes[i].material;
				_meshes[i].material = _damageFlashMaterial;
			}
		}

		_isDamageFlashing = true;

		StartCoroutine(StopDamageFlash());
	}
}
