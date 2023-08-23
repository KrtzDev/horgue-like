using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnIndicator : MonoBehaviour
{
    [SerializeField] private LayerMask _groundLayer;

    public void Destroy()
    {
        Destroy(gameObject);
    }

    private void Start()
    {
        if (Physics.Raycast(transform.position + new Vector3(0, 5, 0), Vector3.down, out RaycastHit rc, _groundLayer))
        {
            transform.position = rc.point;
        }
    }

    private void Update()
    {
        Debug.DrawRay(transform.position + new Vector3(0, 5, 0), Vector3.down, Color.green);
    }
}
