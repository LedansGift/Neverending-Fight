using System;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    private State currentState;

    protected virtual void OnEnable()
    {
        RestartManager.OnResetPhase += ResetStatemachine;
        BossFormManager.OnPhaseFinished += ResetStatemachine;
    }

    protected virtual void OnDisable()
    {
        RestartManager.OnResetPhase -= ResetStatemachine;
        BossFormManager.OnPhaseFinished -= ResetStatemachine;

        StopAllCoroutines();
    }

    void Update()
    {
        currentState?.Tick(Time.deltaTime);
    }

    public virtual void SwitchState(State newState)
    {
        // if (currentState?.GetType() == newState.GetType())
        // {
        //     return;
        // }


        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    protected State GetCurrentState()
    {
        return currentState;
    }

    private void ResetStatemachine()
    {
        SwitchState(null);

        StopAllCoroutines();
    }
}
