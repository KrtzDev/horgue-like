using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    protected AI_Agent_Enemy _enemyType;

    public virtual void Start()
    {
        _enemyType = gameObject.GetComponentInParent<AI_Agent_Enemy>();
    }

    public virtual void DropScore()
    {
        _enemyType.HealthComponent.DropScore();
    }

    public virtual void DropHealthPotion()
    {
        _enemyType.HealthComponent.DropHealthPotion();
    }

    public virtual void SetDeactive()
    {
        _enemyType.SetDeactive();
    }

    public virtual void Shoot()
    {
        _enemyType.Shoot();
    }

    public virtual void DoneShooting()
    {
        _enemyType.DoneShooting();
    }

    public virtual void SetState(AI_StateID state)
    {
        _enemyType.SetState(state);
    }
}