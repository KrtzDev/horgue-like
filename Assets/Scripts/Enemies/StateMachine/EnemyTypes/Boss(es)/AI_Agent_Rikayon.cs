using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Agent_Rikayon : AI_Agent_Enemy
{
    [Header("Boss")]
    public Vector2 _bossStageAnimationMultiplier;
    public Vector2 _bossStageMovementMultiplier;
    [HideInInspector] public int _currentBossStage = 0;
    [HideInInspector] public Vector3Int _lastAbilities = new Vector3Int(0, 0, 0);

    public int _numberOfAttacks;
    public int _numberOfSpecialAttacks;

    [Header("Special Attacks")]
    public Transform _specialAttackSpawnPosition;
    public int _baseSpecialAttackProbability;
    public float _specialAttackProbablityModifier;
    [HideInInspector] public int _currentSpecialAttackProbablity;

    [Header("Spray Attack")]
    [SerializeField] private Vector3 _bossStageSpeedMultiplier_Spray;
    [SerializeField] private Vector3Int _bossStageAngle_Spray;
    public GameObject _sprayAttack_prefab;
    private bool _sprayAttackActive;

    [Header("Spit Attack")]
    public GameObject _spitAttack_prefab;
    [SerializeField, Range(-3, 0)] private float _spitForceMinX;
    [SerializeField, Range(0, 3)] private float _spitForceMaxX;
    [SerializeField, Range(12.5f, 17.5f)] private float _spitForceMinY;
    [SerializeField, Range(17.5f, 22.5f)] private float _spitForceMaxY;
    [SerializeField, Range(-3, 0)] private float _spitForceMinZ;
    [SerializeField, Range(0, 3)] private float _spitForceMaxZ;
    private Vector3 _spitForce;

    [Header("Spike Attack")]
    [SerializeField] private float _spikePreviewTime;
    [SerializeField] private Vector3 _bossStageScaleMultiplier_Spikes;
    public GameObject _spikeAttackInner_prefab;
    public GameObject _spikeAttackMid_prefab;
    public GameObject _spikeAttackOuter_prefab;
    public GameObject _spikePreviewInner_prefab;
    public GameObject _spikePreviewMid_prefab;
    public GameObject _spikePreviewOuter_prefab;
    private int _spikeAttackNumber = 0;

    [Header("Player Damage On Touch")]
    [SerializeField] private int _damageOnTouch;
    [SerializeField] private float _damageTime;
    private float _damageTimer = 0;

    private bool _hasSpitAttacked;

    protected override void Start()
    {
        base.Start();

        OriginalAnimationSpeed = Animator.speed;
        _currentSpecialAttackProbablity = _baseSpecialAttackProbability;

        _hasSpitAttacked = false;
    }

    protected override void Update()
    {
        base.Update();

        if(_damageTime > 0)
            _damageTimer -= Time.deltaTime;
    }

    protected override void RegisterStates()
    {
        StateMachine.RegisterState(new Rikayon_State_ChasePlayer());
        StateMachine.RegisterState(new Rikayon_State_Attack());
        StateMachine.RegisterState(new Rikayon_State_SpecialAttack());
        StateMachine.RegisterState(new Rikayon_State_Retreat());
        StateMachine.RegisterState(new Rikayon_State_Idle());
        StateMachine.RegisterState(new AI_State_Death());
    }

    public override void SetDeactive()
    {
        base.SetDeactive();
    }

    public override void CheckForBossStage()
    {

        if (HealthComponent.currentHealth <= HealthComponent.maxHealth / 2.5 && _currentBossStage == 0)
        {
            transform.LookAt(PlayerTransform);

            Animator.speed = OriginalAnimationSpeed;

            Animator.SetTrigger("bossStage1");
            _currentBossStage = 1;
            HealthComponent.canTakeDamage = false;

            NavMeshAgent.speed *= _bossStageMovementMultiplier.x;
            NavMeshAgent.acceleration *= _bossStageMovementMultiplier.x;

            AudioManager.Instance.PlaySound("RikayonScream");
            gameObject.GetComponent <AI_Agent_Rikayon> ().enabled = false;
        } 
        else if (HealthComponent.currentHealth <= HealthComponent.maxHealth / 4.5 && _currentBossStage == 1)
        {
            transform.LookAt(PlayerTransform);

            Animator.speed = OriginalAnimationSpeed;

            Animator.SetTrigger("bossStage2");
            _currentBossStage = 2;
            HealthComponent.canTakeDamage = false;

            NavMeshAgent.speed /= _bossStageMovementMultiplier.x;
            NavMeshAgent.acceleration /= _bossStageMovementMultiplier.x;

            NavMeshAgent.speed *= _bossStageMovementMultiplier.y;
            NavMeshAgent.acceleration *= _bossStageMovementMultiplier.y;

            AudioManager.Instance.PlaySound("RikayonScream");
            gameObject.GetComponent<AI_Agent_Rikayon>().enabled = false;
        }
    }

    public void SprayAttack()
    {
        // Spray attack in cone shape infront of Rikayon
        // Intimidate 1

        switch (_currentBossStage)
        {
            case 0:
                SprayAttackAbility(_bossStageSpeedMultiplier_Spray.x, _bossStageAngle_Spray.x);
                break;
            case 1:
                SprayAttackAbility(_bossStageSpeedMultiplier_Spray.y, _bossStageAngle_Spray.y);
                break;
            case 2:
                SprayAttackAbility(_bossStageSpeedMultiplier_Spray.z, _bossStageAngle_Spray.z);
                break;
        }

    }

    private void SprayAttackAbility(float currentSpeedMultiplier, int currentAngle)
    {
        if (!_sprayAttackActive)
        {         
            AudioManager.Instance.PlaySound("RikayonGrowl");
            _sprayAttackActive = true;

            GameObject sprayAttack;
            ParticleSystem ps;
            sprayAttack = Instantiate(_sprayAttack_prefab, _specialAttackSpawnPosition);
            ps = sprayAttack.GetComponent<ParticleSystem>();
            ps.Stop();
            var main = ps.main;
            main.startSpeedMultiplier *= currentSpeedMultiplier;
            var shape = ps.shape;
            shape.angle = currentAngle;
            ps.Play();
        }
        else
        {
            _sprayAttackActive = false;
            StartCoroutine(DestroySpawnPositionChilds_IEnumerator(0));
        }
    }

    public void RadialSpikeAttack()
    {
        // spawn Spikes around Rikayon which move periodacilly from inner to outer circle
        // Intimidate 2

        // Spawn Spikes around Rikayon
        // Move Spikes slowly outwards
        // Make Spikes dissapear after x Seconds

        switch (_spikeAttackNumber)
        {
            case 0:
                SpawnSpike();
                break;
            case 1:
                SpawnSpike();
                break;
            case 2:
                SpawnSpike();
                break;
            case 3:
                SpawnSpike();
                break;
        }
    }

    private void SpawnSpike()
    {
        switch (_currentBossStage)
        {
            case 0:
                StartCoroutine(SpawnSpike(_bossStageScaleMultiplier_Spikes.x));
                break;
            case 1:
                StartCoroutine(SpawnSpike(_bossStageScaleMultiplier_Spikes.y));
                break;
            case 2:
                StartCoroutine(SpawnSpike(_bossStageScaleMultiplier_Spikes.z));
                break;
        }
    }

    private IEnumerator SpawnSpike(float currentMultiplier)
    {
        GameObject spikePreview;
        GameObject spikes;

        switch (_spikeAttackNumber)
        {
            case 0:
                AudioManager.Instance.PlaySound("RikayonGrowl");
                DestroySpawnPositionChilds();
                spikePreview = Instantiate(_spikePreviewInner_prefab, _specialAttackSpawnPosition.position, Quaternion.identity, _specialAttackSpawnPosition.transform);
                spikePreview.transform.localScale = new Vector3(spikePreview.transform.localScale.x * currentMultiplier, spikePreview.transform.localScale.y, spikePreview.transform.localScale.z * currentMultiplier);
                yield return new WaitForSeconds(_spikePreviewTime);
                DestroySpawnPositionChilds();
                spikes = Instantiate(_spikeAttackInner_prefab, _specialAttackSpawnPosition.position, Quaternion.identity, _specialAttackSpawnPosition.transform);
                spikes.transform.localScale = new Vector3(spikes.transform.localScale.x * currentMultiplier, spikes.transform.localScale.y, spikes.transform.localScale.z * currentMultiplier);
                _spikeAttackNumber = 1;
                break;
            case 1:
                DestroySpawnPositionChilds();
                spikePreview = Instantiate(_spikePreviewMid_prefab, _specialAttackSpawnPosition.position, Quaternion.identity, _specialAttackSpawnPosition.transform);
                spikePreview.transform.localScale = new Vector3(spikePreview.transform.localScale.x * currentMultiplier, spikePreview.transform.localScale.y, spikePreview.transform.localScale.z * currentMultiplier);
                yield return new WaitForSeconds(_spikePreviewTime);
                DestroySpawnPositionChilds();
                spikes = Instantiate(_spikeAttackMid_prefab, _specialAttackSpawnPosition.position, Quaternion.identity, _specialAttackSpawnPosition.transform);
                spikes.transform.localScale = new Vector3(spikes.transform.localScale.x * currentMultiplier, spikes.transform.localScale.y, spikes.transform.localScale.z * currentMultiplier);
                _spikeAttackNumber = 2;
                break;
            case 2:
                DestroySpawnPositionChilds();
                spikePreview = Instantiate(_spikePreviewOuter_prefab, _specialAttackSpawnPosition.position, Quaternion.identity, _specialAttackSpawnPosition.transform);
                spikePreview.transform.localScale = new Vector3(spikePreview.transform.localScale.x * currentMultiplier, spikePreview.transform.localScale.y, spikePreview.transform.localScale.z * currentMultiplier);
                yield return new WaitForSeconds(_spikePreviewTime);
                DestroySpawnPositionChilds();
                spikes = Instantiate(_spikeAttackOuter_prefab, _specialAttackSpawnPosition.position, Quaternion.identity, _specialAttackSpawnPosition.transform);
                spikes.transform.localScale = new Vector3(spikes.transform.localScale.x * currentMultiplier, spikes.transform.localScale.y, spikes.transform.localScale.z * currentMultiplier);
                _spikeAttackNumber = 3;
                break;
            case 3:
                _spikeAttackNumber = 0;
                DestroySpawnPositionChilds();
                break;
        }

        yield return null;
    }

    private IEnumerator DestroySpawnPositionChilds_IEnumerator(float time)
    {
        yield return new WaitForSeconds(time);
        DestroySpawnPositionChilds();
    }

    public void DestroySpawnPositionChilds()
    {
        for (int i = 0; i < _specialAttackSpawnPosition.childCount; i++)
        {
            Destroy(_specialAttackSpawnPosition.GetChild(i).gameObject);
        }
    }

    public void SpitAttack()
    {
        // Spits semi-lingering toxic waste into the air which falls onto the ground (near the player and on the NavMesh) and stays for a while
        // Intimidate 3

        switch (_currentBossStage)
        {
            case 0:
                StartCoroutine(SpawnSpitAttack(0));
                break;
            case 1:
                StartCoroutine(SpawnSpitAttack(0));
                StartCoroutine(SpawnSpitAttack(0.5f));
                break;
            case 2:
                StartCoroutine(SpawnSpitAttack(0));
                StartCoroutine(SpawnSpitAttack(0.5f));
                StartCoroutine(SpawnSpitAttack(1));
                break;
        }
    }

    private IEnumerator SpawnSpitAttack(float time)
    {
        if(!_hasSpitAttacked)
        {
            AudioManager.Instance.PlaySound("RikayonGrowl");
            _hasSpitAttacked = true;
        }

        yield return new WaitForSeconds(time);

        GameObject spitAttack;

        _spitForce = new Vector3(Random.Range(_spitForceMinX, _spitForceMaxX), Random.Range(_spitForceMinY, _spitForceMaxY), Random.Range(_spitForceMinZ, _spitForceMaxZ));

        spitAttack = Instantiate(_spitAttack_prefab, _specialAttackSpawnPosition.position, Quaternion.identity);
        spitAttack.GetComponent<Rigidbody>().AddForce(_spitForce, ForceMode.Impulse);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && _damageTimer <= 0)
        {
            other.GetComponent<HealthComponent>().TakeDamage(_damageOnTouch, true);
            _damageTimer = _damageTime;
        }
    }

    public void ResetFirstSpitAttack()
    {
        _hasSpitAttacked = false;
    }
}
