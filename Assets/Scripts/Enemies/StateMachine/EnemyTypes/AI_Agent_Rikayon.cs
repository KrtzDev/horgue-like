using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Agent_Rikayon : AI_Agent_Enemy
{
    public int _numberOfAttacks;
    public int _numberOfIntimidations;

    [Header("Special Attacks")]
    public Transform _specialAttackSpawnPosition;

    [Header("Spray Attack")]
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
    public GameObject _spikeAttackInner_prefab;
    public GameObject _spikeAttackMid_prefab;
    public GameObject _spikeAttackOuter_prefab;
    private int _spikeAttackNumber = 0;
    [SerializeField] private float _spikeDestructionWaitTime;


    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void RegisterStates()
    {
        _stateMachine.RegisterState(new Rikayon_State_ChasePlayer());
        _stateMachine.RegisterState(new Rikayon_State_Attack());
        _stateMachine.RegisterState(new Rikayon_State_Retreat());
        _stateMachine.RegisterState(new Rikayon_State_Idle());
        _stateMachine.RegisterState(new AI_State_Death());
    }

    public void SetDeactive()
    {
        this.gameObject.SetActive(false);
    }

    public override void CheckForBossStage()
    {
        if(_healthComponent._currentHealth <= _healthComponent._maxHealth / 2 && _currentBossStage == 0)
        {
            _animator.SetTrigger("bossStage1");
            _currentBossStage = 1;
            _healthComponent._canTakeDamage = false;
            
            gameObject.GetComponent <AI_Agent_Rikayon> ().enabled = false;
        }

        if (_healthComponent._currentHealth <= _healthComponent._maxHealth / 4 && _currentBossStage == 1)
        {
            _animator.SetTrigger("bossStage2");
            _currentBossStage = 2;
            _healthComponent._canTakeDamage = false;
            _healthComponent._maxHealth /= 2;
            gameObject.GetComponent<AI_Agent_Rikayon>().enabled = false;
        }
    }

    public void SprayAttack()
    {
        // Spray attack in cone shape infront of Rikayon
        // Intimidate 1

        if (!_sprayAttackActive)
        {
            _sprayAttackActive = true;

            GameObject sprayAttack;
            sprayAttack = Instantiate(_sprayAttack_prefab, _specialAttackSpawnPosition);
        }
        else
        {
            _sprayAttackActive = false;

            StartCoroutine(DestroySpawnPositionChilds(0));
        }

    }

    public void RadialSpikeAttack()
    {
        // spawn Spikes around Rikayon which move periodacilly from inner to outer circle
        // Intimidate 2

        // Spawn Spikes around Rikayon
        // Move Spikes slowly outwards
        // Make Spikes dissapear after x Seconds

        GameObject spikes;

        switch (_spikeAttackNumber)
        {
            case 0:
                spikes = Instantiate(_spikeAttackInner_prefab, _specialAttackSpawnPosition.position, Quaternion.identity, _specialAttackSpawnPosition);
                _spikeAttackNumber = 1;
                break;
            case 1:
                spikes = Instantiate(_spikeAttackMid_prefab, _specialAttackSpawnPosition.position, Quaternion.identity, _specialAttackSpawnPosition);
                _spikeAttackNumber = 2;
                break;
            case 2:
                spikes = Instantiate(_spikeAttackOuter_prefab, _specialAttackSpawnPosition.position, Quaternion.identity, _specialAttackSpawnPosition);
                _spikeAttackNumber = 3;
                break;
            case 3:
                _spikeAttackNumber = 0;
                StartCoroutine(DestroySpawnPositionChilds(_spikeDestructionWaitTime)); // de-parent Spikes when Spike Script is setup / handle destruction in Spike Script
                break;
        }
    }

    private IEnumerator DestroySpawnPositionChilds(float time)
    {
        yield return new WaitForSeconds(time);

        for (int i = 0; i < _specialAttackSpawnPosition.childCount; i++)
        {
            Destroy(_specialAttackSpawnPosition.GetChild(i).gameObject);
        }
    }

    public void SpitAttack()
    {
        // Spits semi-lingering toxic waste into the air which falls onto the ground (near the player and on the NavMesh) and stays for a while
        // Intimidate 3

        _spitForce = new Vector3(Random.Range(_spitForceMinX, _spitForceMaxX), Random.Range(_spitForceMinY, _spitForceMaxY), Random.Range(_spitForceMinZ, _spitForceMaxZ));

        GameObject spit;
        spit = Instantiate(_spitAttack_prefab, _specialAttackSpawnPosition.position, Quaternion.identity);
        spit.GetComponent<Rigidbody>().AddForce(_spitForce, ForceMode.Impulse);
    }
}
