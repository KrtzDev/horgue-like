using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Agent_Rikayon : AI_Agent_Enemy
{
    public int _numberOfAttacks;
    public int _numberOfIntimidations;

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
        _stateMachine.RegisterState(new Rikayon_State_ChasePlayer());
        _stateMachine.RegisterState(new Rikayon_State_Attack());
        _stateMachine.RegisterState(new Rikayon_State_Retreat());
        _stateMachine.RegisterState(new Rikayon_State_Idle());
        _stateMachine.RegisterState(new AI_State_Death());
    }

    public void SetDeactive()
    {
        this.gameObject.SetActive(false);
    }

    public override void CheckForBossStage()
    {
        if(_healthComponent._currentHealth <= _healthComponent._maxHealth / 2 && _currentBossStage == 0)
        {
            _animator.SetTrigger("bossStage1");
            _currentBossStage = 1;
            _healthComponent._canTakeDamage = false;
            
            gameObject.GetComponent <AI_Agent_Rikayon> ().enabled = false;
        }

        if (_healthComponent._currentHealth <= _healthComponent._maxHealth / 4 && _currentBossStage == 1)
        {
            _animator.SetTrigger("bossStage2");
            _currentBossStage = 2;
            _healthComponent._canTakeDamage = false;
            _healthComponent._maxHealth /= 2;
            gameObject.GetComponent<AI_Agent_Rikayon>().enabled = false;
        }
    }
}
