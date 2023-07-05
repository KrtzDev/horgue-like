using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// https://www.youtube.com/watch?v=1H9jrKyWKs0&t=34s

public class AI_Agent : MonoBehaviour
{
    public AI_StateMachine _stateMachine;
    public AI_StateID _initialState;
    public NavMeshAgent _navMeshAgent;
    public AI_Agent_Config _config;

    private void Start()
    {
        _stateMachine = new AI_StateMachine(this);
        _stateMachine.ChangeState(_initialState);
        _navMeshAgent = GetComponent<NavMeshAgent>();

        // Register States
        _stateMachine.RegisterState(new AI_State_Idle());
        _stateMachine.RegisterState(new AI_State_ChasePlayer());
        _stateMachine.RegisterState(new AI_State_Retreat());
        _stateMachine.RegisterState(new AI_State_Attack());
        _stateMachine.RegisterState(new AI_State_Death());
    }

    private void Update()
    {
        _stateMachine.Update();
    }
}
