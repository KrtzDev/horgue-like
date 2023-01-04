using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAgent : MonoBehaviour
{
    public Transform playerTransform;

    public EnemyStateMachine stateMachine;
    public EnemyStateID initialState;
    public NavMeshAgent navMeshAgent;
    public EnemyAgentConfig config;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        stateMachine = new EnemyStateMachine(this);

        stateMachine.RegisterState(new EnemyIdleState());
        stateMachine.RegisterState(new EnemyDeathState());
        stateMachine.RegisterState(new EnemyChasePlayerState());

        stateMachine.ChangeState(initialState);

        if (playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    private void Update()
    {
        stateMachine.Update();
    }
}
