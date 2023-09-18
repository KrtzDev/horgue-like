using UnityEngine;
using UnityEngine.AI;

// https://www.youtube.com/watch?v=1H9jrKyWKs0&t=34s

[RequireComponent(typeof(NavMeshAgent), typeof(EnemyHealthComponent))]
[DefaultExecutionOrder(1)]
public class AI_Agent_Enemy : AI_Agent
{
    public BasicEnemyData _enemyData;
    public Vector3 _followPosition;
    public bool _isBossEnemy;
    [HideInInspector] public EnemyHealthComponent HealthComponent;

    public int damagePerHit;

    [Header("Height Control")]
    public bool _useHeightControl;
    public GameObject _heightGO;

	public Vector3 DesiredVFXScale => _desiredVFXScale;
	[SerializeField]
	private Vector3 _desiredVFXScale;

	protected override void Start()
    {
        base.Start();

        // Register States

        NavMeshAgent = GetComponent<NavMeshAgent>();
        ObstacleAgent = GetComponent<ObstacleAgent>();
        Animator = GetComponentInChildren<Animator>();
        RigidBody = GetComponent<Rigidbody>();
        HealthComponent = GetComponent<EnemyHealthComponent>();
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerTransform = Player.GetComponent<Transform>();
        DecoyTransform = GameObject.FindGameObjectWithTag("Decoy").transform;

        SetEnemyData();

        StateMachine = new AI_StateMachine(this);
        RegisterStates();
        StateMachine.ChangeState(InitialState);
    }

    protected virtual void Update()
    {
        StateMachine.Update(GetComponent<AI_Agent_Enemy>());
    }

    protected virtual void SetEnemyData()
    {
        NavMeshAgent.speed = _enemyData._maxMoveSpeed;
        NavMeshAgent.acceleration = _enemyData._acceleration;
        AttackTimer = 0f;

        float enemyDamageMultiplier = _enemyData.baseDmgMultiplier + (_enemyData.baseDmgMultiplier * ((GameManager.Instance._currentLevel - 1) * _enemyData.dmgModifier) * (1 + ((GameManager.Instance._currentLevel - 1) * _enemyData.addDmgModifier)));
        float enemyHealthMultiplier = _enemyData.baseHealthMultiplier + (_enemyData.baseHealthMultiplier * ((GameManager.Instance._currentLevel - 1) * _enemyData.healthModifier) * (1 + ((GameManager.Instance._currentLevel - 1) * _enemyData.addHealthModifier)));

        damagePerHit = (int)(_enemyData._damagePerHit * enemyDamageMultiplier);
        HealthComponent.maxHealth = (int)(_enemyData._maxHealth * enemyHealthMultiplier);
        HealthComponent.currentHealth = HealthComponent.maxHealth;
    }

    public virtual void SetDeactive()
    {
        this.gameObject.SetActive(false);
    }

    public virtual void CheckForBossStage()
    {

    }

    public void SetHealthToPercentOfMax(float percent)
    {
        HealthComponent.currentHealth = (int)(HealthComponent.maxHealth * 0.01f * percent);
        if(HealthComponent.currentHealth > HealthComponent.maxHealth)
        {
            HealthComponent.currentHealth = HealthComponent.maxHealth;
        }
        HealthComponent._enemyHealthBar.HandleHealthChanged(percent * 0.01f);
    }

    public virtual void Shoot()
    {
    }

    public virtual void DoneShooting()
    {
    }
}
