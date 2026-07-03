using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Boss Idle", menuName = "Boss Attack/Idle", order = 5)]
public class BossIdleNode : BossAttackNode
{
    [SerializeField]
    private float idleTime;

    public override void PerformAttack(
        BossAttackManager attacker,
        Action OnAttackFinished,
        float damageMultiplier = 1
    )
    {
        this.OnAttackFinished = OnAttackFinished;
        attacker.StartBossIdle(idleTime, FinishIdle);
    }

    private void FinishIdle()
    {
        FinishAttack();
    }
}
