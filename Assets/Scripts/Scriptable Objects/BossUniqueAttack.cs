using System;
using UnityEngine;

public class BossUniqueAttack : BossAttackNode
{
    protected int uniqueAttackIndex = 0;

    private BossAttackManager attacker;

    public override void PerformAttack(
        BossAttackManager attacker,
        Action OnAttackFinished,
        float damageMultiplier = 1
    )
    {
        this.attacker = attacker;
        this.OnAttackFinished = OnAttackFinished;

        PlayFX(attacker.transform);

        StateDictionary stateDictionary = attacker.GetStateDictionary();
        BossStateMachine stateMachine = stateDictionary.GetStateMachine() as BossStateMachine;

        if (stateDictionary.TryGetState(uniqueAttackIndex, out State state))
        {
            BossState bossState = state as BossState;
            bossState.SetStateFinished(DelayedAttackFinish, GetAttackName(), damageMultiplier);

            stateMachine.SwitchState(state);
        }
    }

    private void DelayedAttackFinish()
    {
        attacker.StartBossIdle(0.01f, FinishAttack);
    }

    public override void FinishAttack()
    {
        if (failableAttack)
        {
            Debug.Log("Attack fail check");
            OnAttackFailCheck?.Invoke(this, GetAttackName());
        }

        base.FinishAttack();
    }
}
