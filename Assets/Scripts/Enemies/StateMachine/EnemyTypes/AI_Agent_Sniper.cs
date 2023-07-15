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

    protected override void RegisterStates()
    {
        _stateMachine.RegisterState(new Sniper_State_Idle());
        _stateMachine.RegisterState(new Sniper_State_ChasePlayer());
        _stateMachine.RegisterState(new Sniper_State_Retreat());
        _stateMachine.RegisterState(new Sniper_State_Attack());
        _stateMachine.RegisterState(new AI_State_Death());
    }

    public void SetDeactive()
    {
        this.gameObject.SetActive(false);
        AI_Manager.Instance.Sniper.Remove(this);
    }

    public void Shoot()
    {
        Rigidbody rb = Instantiate(_projectile, ProjectilePoint.position, Quaternion.identity).GetComponent<Rigidbody>();
        rb.AddForce(TargetDirection * _projectileSpeed, ForceMode.Impulse);
    }

    public void DoneShooting()
    {
        this.GetComponent<Animator>().SetBool("isShooting", false);
    }
}
