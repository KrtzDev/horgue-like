using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] private Material _damageFlashMaterial;
    [SerializeField] private float _damageFlashTime;
    private bool _isDamageFlashing;
    private Material[] _originalMaterials;
    private SkinnedMeshRenderer[] _skinnedMeshes;
    private MeshRenderer[] _meshes;

    [Header("Height Control")]
    public bool _useHeightControl;
    public GameObject _heightGO;

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

        DeclareMeshes();
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

        float enemyDamageMultiplier = _enemyData.baseDmgMultiplier + (_enemyData.baseDmgMultiplier * (GameManager.Instance._currentLevelArray * _enemyData.dmgModifier) * (1 + (GameManager.Instance._currentLevelArray * _enemyData.addDmgModifier)));
        float enemyHealthMultiplier = _enemyData.baseHealthMultiplier + (_enemyData.baseHealthMultiplier * (GameManager.Instance._currentLevelArray * _enemyData.healthModifier) * (1 + (GameManager.Instance._currentLevelArray * _enemyData.addHealthModifier)));

        damagePerHit = (int)(_enemyData._damagePerHit * enemyDamageMultiplier);
        HealthComponent._maxHealth = (int)(_enemyData._maxHealth * enemyHealthMultiplier);
        HealthComponent._currentHealth = HealthComponent._maxHealth;
    }

    public virtual void SetDeactive()
    {
        this.gameObject.SetActive(false);
    }

    public virtual void CheckForBossStage()
    {

    }

    private void SetHealthToPercentOfMax(float percent)
    {
        HealthComponent._currentHealth = (int)(HealthComponent._maxHealth * percent);
        if(HealthComponent._currentHealth > HealthComponent._maxHealth)
        {
            HealthComponent._currentHealth = HealthComponent._maxHealth;
        }
        HealthComponent._enemyHealthBar.HandleHealthChanged(percent);
    }

    public virtual void Shoot()
    {
    }

    public virtual void DoneShooting()
    {
    }

    private void DeclareMeshes()
    {
        if (GetComponentInChildren<SkinnedMeshRenderer>() != null)
        {
            _skinnedMeshes = GetComponentsInChildren<SkinnedMeshRenderer>();
            _originalMaterials = new Material[_skinnedMeshes.Length];
        }
        else
        {
            _meshes = GetComponentsInChildren<MeshRenderer>();
            _originalMaterials = new Material[_meshes.Length];
        }

        _isDamageFlashing = false;
    }

    public virtual void DamageFlash()
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
        else
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

    private IEnumerator StopDamageFlash()
    {
        yield return new WaitForSeconds(_damageFlashTime);

        if (GetComponentInChildren<SkinnedMeshRenderer>() != null)
        {
            for (int i = 0; i < _skinnedMeshes.Length; i++)
            {
                _skinnedMeshes[i].material = _originalMaterials[i];
            }
        }
        else
        {
            for (int i = 0; i < _meshes.Length; i++)
            {
                _meshes[i].material = _originalMaterials[i];
            }
        }

        yield return new WaitForSeconds(_damageFlashTime);

        _isDamageFlashing = false;
    }
}
