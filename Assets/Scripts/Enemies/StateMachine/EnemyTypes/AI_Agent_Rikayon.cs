using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Agent_Rikayon : AI_Agent_Enemy
{
    protected override void Start()
    {
        base.Start();

        AI_Manager.Instance.PasuKan.Add(this);
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void RegisterStates()
    {
        _stateMachine.RegisterState(new AI_State_Death());
    }

    public void SetDeactive()
    {
        this.gameObject.SetActive(false);
        AI_Manager.Instance.PasuKan.Remove(this);
    }
}
