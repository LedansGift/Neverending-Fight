using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(
    fileName = "New Boss Phase Conditional",
    menuName = "Boss Phase Conditional",
    order = 1
)]
public class BossPhaseConditional : BossPhase
{
    private int phaseConditionalResult;

    [SerializeField]
    private int formConditionalIndex;

    [SerializeField]
    private BossPhase[] alternatePhases;

    public override BossNode[] GetAttackPattern()
    {
        if (phaseConditionalResult == 0)
        {
            return base.GetAttackPattern();
        }
        else
        {
            return alternatePhases[phaseConditionalResult - 1].GetAttackPattern();
        }
    }

    public override HealthThresholdPhaseChange GetHealthPhaseChange()
    {
        if (phaseConditionalResult == 0)
        {
            return base.GetHealthPhaseChange();
        }
        else
        {
            return alternatePhases[phaseConditionalResult - 1].GetHealthPhaseChange();
        }
    }

    public override BattleStatePhaseChange GetBattleStatePhaseChange()
    {
        if (phaseConditionalResult == 0)
        {
            return base.GetBattleStatePhaseChange();
        }
        else
        {
            return alternatePhases[phaseConditionalResult - 1].GetBattleStatePhaseChange();
        }
    }

    public override void DeactivateBossPhase()
    {
        if (phaseConditionalResult == 0)
        {
            base.DeactivateBossPhase();
        }
        else
        {
            alternatePhases[phaseConditionalResult - 1].DeactivateBossPhase();
        }
    }

    public override void InitialiseBossPhase(BossPhaseManager bossPhaseManager)
    {
        phaseConditionalResult = bossPhaseManager
            .GetConditionalManager()
            .ResolveConditional(formConditionalIndex);

        if (phaseConditionalResult == 0)
        {
            base.InitialiseBossPhase(bossPhaseManager);
        }
        else
        {
            alternatePhases[phaseConditionalResult - 1].InitialiseBossPhase(bossPhaseManager);
        }
    }
}
