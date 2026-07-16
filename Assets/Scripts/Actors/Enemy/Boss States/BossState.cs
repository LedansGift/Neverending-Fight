using System;
using UnityEngine;

public abstract class BossState : State
{
    protected float damageMult = 1f;
    protected Action OnStateFinished;
    protected BossStateMachine bossStateMachine;

    protected BossState(BossStateMachine stateMachine)
        : base(stateMachine)
    {
        bossStateMachine = stateMachine;
    }

    public void SetStateFinished(Action OnStateFinished, float damageMult = 1f)
    {
        this.OnStateFinished = OnStateFinished;
        this.damageMult = damageMult;
    }
}
