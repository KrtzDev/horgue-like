using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Agent_PasuKan : AI_Agent
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
        _stateMachine.RegisterState(new PasuKan_State_Idle());
        _stateMachine.RegisterState(new PasuKan_State_ChasePlayer());
        _stateMachine.RegisterState(new PasuKan_State_Attack());
        _stateMachine.RegisterState(new AI_State_Death());
        _stateMachine.RegisterState(new AI_State_Damage());
    }

    public void SetDeactive()
    {
        this.gameObject.SetActive(false);
    }
}
