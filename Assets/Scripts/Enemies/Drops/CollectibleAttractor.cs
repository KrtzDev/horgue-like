using UnityEngine;

public class CollectibleAttractor : MonoBehaviour
{
    public float attractorSpeed;
    public bool moveToPlayer;

    [HideInInspector] public Collider playerCollider;

    [SerializeField] private Collectible _collectible;
    [SerializeField] private bool _healthPack;

    private void Awake()
    {
        moveToPlayer = false;
    }

    private void Update()
    {
        if(moveToPlayer)
        {
            MoveToPlayer(playerCollider);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !_collectible._wasPickedUp)
        {
            if(_healthPack)
            {
                if(other.GetComponent<HealthComponent>().currentHealth < other.GetComponent<HealthComponent>().maxHealth)
                {
                    MoveToPlayer(other);
                }
            }
            else
            {
                MoveToPlayer(other);
            }
        }
    }

    public void MoveToPlayer(Collider other)
    {
        Vector3 targetPos = other.transform.position;

        transform.position = Vector3.MoveTowards(transform.position, targetPos, attractorSpeed * Time.deltaTime);
    }
}
