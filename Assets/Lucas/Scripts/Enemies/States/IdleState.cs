using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    protected D_Entity _stateData;

    public IdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_Entity stateData) : base(entity, stateMachine, animBoolName)
    {
        this._stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
