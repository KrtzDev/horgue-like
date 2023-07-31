using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone_State_Death : AI_State_Death
{

    public override void Enter(AI_Agent agent)
    {
        base.Enter(agent);

        agent._rb.isKinematic = false;
        agent._rb.useGravity = true;
        agent.GetComponent<Collider>().isTrigger = true;
    }

    public override void Update(AI_Agent agent)
    {
        base.Update(agent);
    }

    public override void Exit(AI_Agent agent)
    {
        base.Exit(agent);
    }
}
