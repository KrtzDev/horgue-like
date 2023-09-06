using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleAttractor : MonoBehaviour
{
    [SerializeField] private float _attractorSpeed;
    [SerializeField] private Collectible _collectible;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !_collectible._wasPickedUp)
        {
            Vector3 targetPos = other.transform.position;

            if (transform.position.y > other.transform.position.y)
            {
                targetPos = new(other.transform.position.x, transform.position.y, other.transform.position.z);
            }

            transform.position = Vector3.MoveTowards(transform.position, targetPos, _attractorSpeed * Time.deltaTime);
        }
    }
}
