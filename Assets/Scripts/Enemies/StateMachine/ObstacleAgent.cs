using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(NavMeshObstacle))]
public class ObstacleAgent : MonoBehaviour
{
    [SerializeField] private float _carvingTime = 0.5f;
    [SerializeField] private float _carvingMoveThreshold = 0.1f;

    private NavMeshAgent _agent;
    private NavMeshObstacle _obstacle;

    private float _lastMoveTime;
    private Vector3 _lastPosition;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _obstacle = GetComponent<NavMeshObstacle>();

        _obstacle.enabled = false;
        _obstacle.carveOnlyStationary = false;
        _obstacle.carveOnlyStationary = true;

        _lastPosition = transform.position;
    }

    private void Update()
    {
        if(Vector3.Distance(_lastPosition, transform.position) > _carvingMoveThreshold)
        {
            _lastMoveTime = Time.time;
            _lastPosition = transform.position;
        }

        if(_lastMoveTime + _carvingTime < Time.time)
        {
            _agent.enabled = false;
            _obstacle.enabled = true;
        }
    }

    public void SetDestination(Vector3 position)
    {
        _obstacle.enabled = false;

        _lastMoveTime = Time.time;
        _lastPosition = transform.position;

        StartCoroutine(MoveAgent(position));
    }

    private IEnumerator MoveAgent(Vector3 position)
    {
        yield return null;

        _agent.enabled = true;
        _agent.SetDestination(position);
    }
}
