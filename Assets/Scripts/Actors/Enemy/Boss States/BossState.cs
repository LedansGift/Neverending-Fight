using System;
using UnityEngine;

public abstract class BossState : State
{
    protected Action OnStateFinished;

    protected BossState(BossStateMachine stateMachine)
        : base(stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public void SetStateFinished(Action OnStateFinished)
    {
        this.OnStateFinished = OnStateFinished;

        Debug.Log("StateFinished Set for ");
    }
}
