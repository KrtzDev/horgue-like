using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Agent_Drone : AI_Agent_Enemy
{
    [Header("Height Control")]
    [SerializeField] private GameObject _droneHeightGO;
    [SerializeField] private float _minHeightAboveGround;
    [SerializeField] private float _maxHeightAboveGround;
    private SphereCollider _detectionCollider;

    [Header("Projectile")]
    [SerializeField] private GameObject _projectile;
    [field: SerializeField] public Transform ProjectilePoint { get; private set; }
    [field: SerializeField] public Transform ProjectilePoint1 { get; private set; }
    [field: SerializeField] public Transform ProjectilePoint2 { get; private set; }
    [SerializeField] private float _projectileSpeed;
    [HideInInspector] public Vector3 TargetDirection { get; set; }

    protected override void Start()
    {
        base.Start();

        AI_Manager.Instance.Drone.Add(this);

        _detectionCollider = GetComponent<SphereCollider>();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void SetEnemyData()
    {
        base.SetEnemyData();

        _projectile.GetComponent<EnemyProjectile>().baseDamage = _enemyData._damagePerHit;
    }

    protected override void RegisterStates()
    {
        _stateMachine.RegisterState(new Drone_State_Idle());
        _stateMachine.RegisterState(new Drone_State_ChasePlayer());
        _stateMachine.RegisterState(new Drone_State_Attack());
        _stateMachine.RegisterState(new AI_State_Death());
    }

    public void SetDeactive()
    {
        this.gameObject.SetActive(false);
        AI_Manager.Instance.RangedRobot.Remove(this);
    }

    public void Shoot()
    {
        Rigidbody rb1 = Instantiate(_projectile, ProjectilePoint1.position, Quaternion.identity).GetComponent<Rigidbody>();
        rb1.AddForce(TargetDirection * _projectileSpeed, ForceMode.Impulse);

        Rigidbody rb2 = Instantiate(_projectile, ProjectilePoint2.position, Quaternion.identity).GetComponent<Rigidbody>();
        rb2.AddForce(TargetDirection * _projectileSpeed, ForceMode.Impulse);
    }

    public void DoneShooting()
    {
        this.GetComponent<Animator>().SetBool("isShooting", false);
        _stateMachine.ChangeState(AI_StateID.Idle);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Death") && other.CompareTag("Ground"))
        {
            SetDeactive();
        }
    }
}
