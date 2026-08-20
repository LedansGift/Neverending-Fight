using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Boss Cast", menuName = "Boss Attack/Cast", order = 8)]
public class BossCastNode : BossAttackNode
{
    [SerializeField]
    private float castTime;

    [SerializeField]
    private string castName;

    public override void PerformAttack(
        BossAttackManager attacker,
        Action OnAttackFinished,
        float damageMultiplier = 1
    )
    {
        this.OnAttackFinished = OnAttackFinished;
        BossCastBarUI.InitiateCastEvent(new CastInfo(castName, castTime));

        FinishCast();
    }

    private void FinishCast()
    {
        FinishAttack();
    }
}
