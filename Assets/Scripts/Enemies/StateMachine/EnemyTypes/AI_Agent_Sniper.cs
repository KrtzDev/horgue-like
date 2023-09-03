using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Agent_Sniper : AI_Agent_Enemy
{
    [Header("Projectile")]
    [SerializeField] private GameObject _projectile;
    [field: SerializeField] public Transform ProjectilePoint { get; private set; }
    [SerializeField] private float _projectileSpeed;
    [HideInInspector] public Vector3 TargetDirection { get; set; }

    protected override void Start()
    {
        base.Start();

        AI_Manager.Instance.Sniper.Add(this);
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void SetEnemyData()
    {
        base.SetEnemyData();

        _projectile.GetComponent<EnemyProjectile>().baseDamage = damagePerHit;
    }

    protected override void RegisterStates()
    {
        StateMachine.RegisterState(new Sniper_State_Idle());
        StateMachine.RegisterState(new Sniper_State_ChasePlayer());
        StateMachine.RegisterState(new Sniper_State_Retreat());
        StateMachine.RegisterState(new Sniper_State_Attack());
        StateMachine.RegisterState(new AI_State_Death());
    }

    public override void SetDeactive()
    {
        base.SetDeactive();
        AI_Manager.Instance.Sniper.Remove(this);
    }

    public override void Shoot()
    {
        Rigidbody rb = Instantiate(_projectile, ProjectilePoint.position, Quaternion.identity).GetComponent<Rigidbody>();
        rb.AddForce(TargetDirection * _projectileSpeed, ForceMode.Impulse);
    }

    public override void DoneShooting()
    {
        Animator.SetBool("isShooting", false);
        Animator.SetBool("isAttacking", false);
        StateMachine.ChangeState(AI_StateID.Idle);
    }
}
