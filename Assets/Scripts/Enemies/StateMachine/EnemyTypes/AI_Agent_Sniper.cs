using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Agent_Sniper : AI_Agent
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void RegisterStates()
    {
        _stateMachine.RegisterState(new Sniper_State_Idle());
        _stateMachine.RegisterState(new Sniper_State_ChasePlayer());
        _stateMachine.RegisterState(new Sniper_State_Retreat());
        _stateMachine.RegisterState(new Sniper_State_Attack());
        _stateMachine.RegisterState(new Sniper_State_Death());
    }
}
