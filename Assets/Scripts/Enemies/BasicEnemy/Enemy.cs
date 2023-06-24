using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
	[SerializeField] private NavMeshAgent _agent;

	[Header("Projectile")]
	[SerializeField] private GameObject _projectile;
    [field: SerializeField] public Transform ProjectilePoint { get; private set; }
    [SerializeField] private float _projectileSpeed;

    [field: SerializeField] public LayerMask GroundLayer { get; private set; }
    [field: SerializeField] public LayerMask EnemyLayer { get; private set; }
    [field: SerializeField] public LayerMask PlayerLayer { get; private set; }

    [field: SerializeField] public BasicEnemyData EnemyData { get; set; }

    public bool FollowDecoy { get; set; }
    [HideInInspector] public Vector3 TargetDirection { get; set; }


    public void Awake()
    {
        _agent.speed = EnemyData._maxMoveSpeed;
        _agent.acceleration = EnemyData._acceleration;
    }


    public void Start()
    {
        if(_projectile != null)
        {
            _projectile.GetComponent<EnemyProjectile>().baseDamage = gameObject.GetComponent<Enemy>().EnemyData._damagePerHit;
        }
    }

    public void OnDisable()
    {
        _agent.enabled = false;
    }

    public void Shoot()
    {
        Rigidbody rb = Instantiate(_projectile, ProjectilePoint.position, Quaternion.identity).GetComponent<Rigidbody>();
        rb.AddForce(TargetDirection * _projectileSpeed, ForceMode.Impulse);
    }

    public void DoneShooting()
    {
        _agent.GetComponent<Animator>().SetBool("isShooting", false);
    }

    public void SetDeactive()
    {
        gameObject.SetActive(false);
    }
}
