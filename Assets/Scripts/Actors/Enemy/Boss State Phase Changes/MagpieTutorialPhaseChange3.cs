using System;
using UnityEngine;

public class MagpieTutorialPhaseChange3 : BattleState
{
    private bool enemiesKilled = false;

    [SerializeField]
    private BossPhase nextTutorialPhase;

    public static EventHandler<bool> OnToggleTomeBarriers;

    public override BossPhase GetNewPhase()
    {
        return nextTutorialPhase;
    }

    public override void ResetBattleState() { }

    public override bool ResolveBattleState()
    {
        if (enemiesKilled)
        {
            OnToggleTomeBarriers?.Invoke(this, false);
        }

        return enemiesKilled;
    }

    public override void ActivateListeners()
    {
        base.ActivateListeners();

        OnToggleTomeBarriers?.Invoke(this, true);

        ProjectileEntityController.OnNewActiveEntities += EvaluateActiveEntities;
    }

    public override void DeactivateListeners()
    {
        base.DeactivateListeners();

        ProjectileEntityController.OnNewActiveEntities -= EvaluateActiveEntities;
    }

    private void EvaluateActiveEntities(object sender, int entityNumber)
    {
        if (entityNumber <= 0)
        {
            Debug.Log("All crows killed");
            enemiesKilled = true;
        }
    }
}
