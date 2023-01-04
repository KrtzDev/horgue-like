using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMelee : Entity
{
    public TankMelee_IdleState idleState { get; private set; }
    public TankMelee_MoveState moveState { get; private set; }

    public override void Start()
    {
        base.Start();

        idleState = new TankMelee_IdleState(this, stateMachine, "idle", entityData, this);
        moveState = new TankMelee_MoveState(this, stateMachine, "move", entityData, this);
    }
}
