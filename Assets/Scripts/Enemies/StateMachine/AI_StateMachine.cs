using UnityEngine;

// https://www.youtube.com/watch?v=1H9jrKyWKs0&t=34s

public class AI_StateMachine
{
    public AI_State[] _states;
    public AI_Agent_Enemy _agent;
    public AI_StateID _currentState;

    public AI_StateMachine(AI_Agent_Enemy agent)
    {
        this._agent = agent;
        int numStates = System.Enum.GetNames(typeof(AI_StateID)).Length;
        _states = new AI_State[numStates];
    }

    public void RegisterState(AI_State state)
    {
        int index = (int)state.GetID();
        _states[index] = state;

    }

    public AI_State GetState(AI_StateID stateID)
    {
        int index = (int)stateID;
        return _states[index];
    }

    public void Update(AI_Agent_Enemy agent)
    {
        if(agent.enabled)
        { 
            GetState(_currentState)?.Update(_agent);
        }
    }

    public void ChangeState(AI_StateID newstate)
    {
        GetState(_currentState)?.Exit(_agent);
        _currentState = newstate;
        GetState(_currentState)?.Enter(_agent);
    }
}
