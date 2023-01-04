using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackState : State
{
    protected D_Entity _stateData;

    public RangedAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_Entity stateData) : base(entity, stateMachine, animBoolName)
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
