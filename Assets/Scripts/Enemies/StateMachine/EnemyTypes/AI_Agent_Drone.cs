using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Agent_Drone : AI_Agent_Enemy
{
    [Header("Height Control")]
    [SerializeField] private GameObject _droneHeightGO;
    [SerializeField] private float _minHeightAboveGround;
    [SerializeField] private float _maxHeightAboveGround;
    private AnimationCurve _heightAboveGround;
    private SphereCollider _detectionCollider;
    private Vector3 _baseColliderCenter;

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
        _baseColliderCenter = _detectionCollider.center;

        _heightAboveGround = new AnimationCurve(new Keyframe(0, _minHeightAboveGround), new Keyframe(1, _maxHeightAboveGround));
        _heightAboveGround.preWrapMode = WrapMode.PingPong;
        _heightAboveGround.postWrapMode = WrapMode.PingPong;
    }

    protected override void Update()
    {
        base.Update();

        CheckHeight();
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

    public void DetermineTargetPosition(AI_Agent_Drone drone, Vector3 followPosition)
    {
        TargetDirection = (followPosition + new Vector3(0, 0.5f, 0) - drone.transform.position - new Vector3(0, drone._droneHeightGO.transform.position.y, 0)).normalized;
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

    private void CheckHeight()
    {
        // check how much above ground

        RaycastHit hit;
        Vector3 heightPos = new Vector3(transform.position.x, _droneHeightGO.transform.position.y + transform.position.y, transform.position.z);

        if (Physics.Raycast(heightPos, Vector3.down, out hit, Mathf.Infinity, _player.GetComponent<PlayerMovementMobility>()._groundLayer))
        {
            if(hit.distance < _minHeightAboveGround || hit.distance > _maxHeightAboveGround)
            {
                float random = Random.Range(0.0f, 1.0f);
                float yPos = hit.point.y +  _heightAboveGround.Evaluate(random);
                heightPos = new Vector3(_droneHeightGO.transform.position.x, yPos, _droneHeightGO.transform.position.z);
                _droneHeightGO.transform.position = heightPos;
                _detectionCollider.center = _baseColliderCenter + heightPos;
            }
        }
    }
}
