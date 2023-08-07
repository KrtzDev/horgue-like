using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Agent_Orc : AI_Agent_Enemy
{
    protected override void Start()
    {
        base.Start();

        AI_Manager.Instance.Orc.Add(this);
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void RegisterStates()
    {
        _stateMachine.RegisterState(new Orc_State_Idle());
        _stateMachine.RegisterState(new Orc_State_ChasePlayer());
        _stateMachine.RegisterState(new Orc_State_Attack());
        _stateMachine.RegisterState(new AI_State_Death());
    }

    public override void SetDeactive()
    {
        base.SetDeactive();
        AI_Manager.Instance.PasuKan.Remove(this);
    }
}
