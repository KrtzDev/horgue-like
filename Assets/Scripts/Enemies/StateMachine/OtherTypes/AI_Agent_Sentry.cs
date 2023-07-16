using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Agent_Sentry : AI_Agent
{

    protected override void Start()
    {
        // Register States
        _animator = GetComponent<Animator>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerTransform = _player.GetComponent<Transform>();
        _decoyTransform = GameObject.FindGameObjectWithTag("Decoy").transform;

        _stateMachine = new AI_StateMachine(this);
        RegisterStates();
        _stateMachine.ChangeState(_initialState);
    }

    protected virtual void Update()
    {
        _stateMachine.Update(GetComponent<AI_Agent_Enemy>());
    }
}
