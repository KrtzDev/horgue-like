using UnityEngine;
using UnityEngine.AI;

public class Enemy : PoolableObject
{
    [field: SerializeField] public BasicEnemyData enemyData { get; set; }
    [field: SerializeField] public NavMeshAgent Agent { get; set; }
    public bool FollowDecoy { get; set; }

    [field: SerializeField] public GameObject projectile { get; private set; }
    [field: SerializeField] public Transform projectilePoint { get; private set; }
    [SerializeField] private float projectileSpeed;

    [field: SerializeField] public LayerMask GroundLayer { get; private set; }
    [field: SerializeField] public LayerMask EnemyLayer { get; private set; }

    public override void OnDisable()
    {
        base.OnDisable();

        Agent.enabled = false;
    }

    public void Shoot()
    {
        Rigidbody rb = Instantiate(projectile, projectilePoint.position, Quaternion.identity).GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * projectileSpeed, ForceMode.Impulse);
    }

    public void SetDeactive()
    {
        this.gameObject.SetActive(false);
    }
}
