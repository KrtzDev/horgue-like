using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : State
{
    protected D_Entity stateData;

    public DeathState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_Entity stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
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
