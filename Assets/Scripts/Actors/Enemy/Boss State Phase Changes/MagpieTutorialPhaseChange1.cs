using UnityEngine;

public class MagpieTutorialPhaseChange1 : BattleState
{
    private bool whirlwindDodged = false;

    [SerializeField]
    private BossPhase nextTutorialPhase;

    public override BossPhase GetNewPhase()
    {
        return nextTutorialPhase;
    }

    public override void ResetBattleState() { }

    public override bool ResolveBattleState()
    {
        //if whirlwind successfully dodges with special, return true

        return whirlwindDodged;
    }

    public override void ActivateListeners()
    {
        base.ActivateListeners();

        Debug.Log("Listener subscription");

        BossAttackManager.OnAttackFailed += CheckAttackFail;
    }

    public override void DeactivateListeners()
    {
        base.DeactivateListeners();

        BossAttackManager.OnAttackFailed -= CheckAttackFail;
    }

    private void CheckAttackFail(object sender, bool failed)
    {
        Debug.Log("Attack Failed: " + failed);

        if (!failed)
        {
            whirlwindDodged = true;
        }
    }
}
