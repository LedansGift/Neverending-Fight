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

    public override BossAttackNode[] GetAttackPattern()
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

    public override void InitialiseBossPhase(BossFormManager bossFormManager)
    {
        phaseConditionalResult = bossFormManager
            .GetConditionalManager()
            .ResolveConditional(formConditionalIndex);
    }
}
