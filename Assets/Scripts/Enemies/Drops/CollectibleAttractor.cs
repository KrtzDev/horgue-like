using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleAttractor : MonoBehaviour
{
    public float attractorSpeed;
    [SerializeField] private Collectible _collectible;
    [HideInInspector] public Collider playerCollider;
    [SerializeField] private bool healthPack;
    public bool moveToPlayer;

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
            if(healthPack)
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
