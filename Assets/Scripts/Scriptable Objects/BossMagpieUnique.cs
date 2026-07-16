using System;
using UnityEngine;

[CreateAssetMenu(
    fileName = "Boss Unique Attack",
    menuName = "Boss Attack/Unique Attack/Magpie",
    order = 1
)]
public class BossMagpieUnique : BossAttackNode
{
    [SerializeField]
    private MagpieUniqueAttacks magpieUniqueAttack;

    public override void PerformAttack(
        BossAttackManager attacker,
        Action OnAttackFinished,
        float damageMultiplier = 1
    )
    {
        this.OnAttackFinished = OnAttackFinished;

        StateDictionary stateDictionary = attacker.GetStateDictionary();
        BossStateMachine stateMachine = stateDictionary.GetStateMachine() as BossStateMachine;

        if (stateDictionary.TryGetState((int)magpieUniqueAttack, out State state))
        {
            //also send in onattackfinished action

            // if (state.GetType() == typeof(BossState))
            // {
            //     Debug.Log("Boss State Recognised");
            BossState bossState = state as BossState;
            bossState.SetStateFinished(FinishAttack, damageMultiplier);
            //}

            stateMachine.SwitchState(state);
        }
    }

    public override void FinishAttack()
    {
        OnAttackFailCheck?.Invoke(this, EventArgs.Empty);

        base.FinishAttack();
    }
}
