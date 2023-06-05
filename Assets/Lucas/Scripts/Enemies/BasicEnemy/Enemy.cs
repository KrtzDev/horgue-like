using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [field: SerializeField] public BasicEnemyData EnemyData { get; set; }
    [field: SerializeField] public NavMeshAgent Agent { get; set; }
    public bool FollowDecoy { get; set; }

    [field: SerializeField] public GameObject Projectile { get; private set; }
    [field: SerializeField] public Transform ProjectilePoint { get; private set; }
    [SerializeField] private float _projectileSpeed;

    [field: SerializeField] public LayerMask GroundLayer { get; private set; }
    [field: SerializeField] public LayerMask EnemyLayer { get; private set; }
    [field: SerializeField] public LayerMask PlayerLayer { get; private set; }

    [HideInInspector] public Vector3 TargetDirection { get; set; }

    public void Start()
    {
        if(Projectile != null)
        {
            Projectile.GetComponent<EnemyProjectile>().baseDamage = this.gameObject.GetComponent<Enemy>().EnemyData._damagePerHit;
        }
    }

    public void OnDisable()
    {
        Agent.enabled = false;
    }

    public void Shoot()
    {
        Rigidbody rb = Instantiate(Projectile, ProjectilePoint.position, Quaternion.identity).GetComponent<Rigidbody>();
        rb.AddForce(TargetDirection * _projectileSpeed, ForceMode.Impulse);
    }

    public void DoneShooting()
    {
        Agent.GetComponent<Animator>().SetBool("isShooting", false);
    }

    public void SetDeactive()
    {
        this.gameObject.SetActive(false);
    }
}
