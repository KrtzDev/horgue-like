using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://www.youtube.com/watch?v=1H9jrKyWKs0&t=34s

public enum AI_StateID
{
    Idle,
    ChasePlayer,
    Retreat,
    Attack,
    SpecialAttack,
    Death,
}

public interface AI_State
{
    AI_StateID GetID();
    void Enter(AI_Agent agent);
    void Update(AI_Agent agent);
    void Exit(AI_Agent agent);
}

