using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleAttractor : MonoBehaviour
{
    public float attractorSpeed;
    [SerializeField] private Collectible _collectible;
    public Collider playerCollider;
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
            MoveToPlayer(other);
        }
    }

    public void MoveToPlayer(Collider other)
    {
        Vector3 targetPos = other.transform.position;

        transform.position = Vector3.MoveTowards(transform.position, targetPos, attractorSpeed * Time.deltaTime);
    }
}
