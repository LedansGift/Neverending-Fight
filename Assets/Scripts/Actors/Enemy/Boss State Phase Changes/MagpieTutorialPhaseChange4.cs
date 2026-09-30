using System;
using UnityEngine;

public class MagpieTutorialPhaseChange4 : BattleState
{
    private int damageThreshold = 15;
    private bool highDamageDealt = false;

    [SerializeField]
    private BossPhase nextTutorialPhase;

    public override BossPhase GetNewPhase()
    {
        return nextTutorialPhase;
    }

    public override void ResetBattleState() { }

    public override bool ResolveBattleState()
    {
        return highDamageDealt;
    }

    public override void ActivateListeners()
    {
        base.ActivateListeners();

        DifferentialShield.OnShieldDamageResolved += EvaluateDamageResolved;
    }

    public override void DeactivateListeners()
    {
        base.DeactivateListeners();

        DifferentialShield.OnShieldDamageResolved -= EvaluateDamageResolved;
    }

    private void EvaluateDamageResolved(object sender, int damageDealt)
    {
        if (damageDealt > damageThreshold)
        {
            highDamageDealt = true;
        }
    }
}
