using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMelee_MoveState : MoveState
{
    private TankMelee _tankMelee;

    public TankMelee_MoveState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_Entity stateData, TankMelee tankMelee) : base(entity, stateMachine, animBoolName, stateData)
    {
        this._tankMelee = tankMelee;
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
