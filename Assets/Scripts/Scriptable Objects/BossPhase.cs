using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New Boss Phase", menuName = "Boss Phase", order = 0)]
public class BossPhase : ScriptableObject
{
    [SerializeField]
    protected BossNode[] bossAttackPattern;

    [SerializeField]
    protected BossPhase endOfPatternPhaseChange;

    [SerializeField]
    protected HealthThresholdPhaseChange healthPhaseChange;

    [SerializeField]
    protected BattleStatePhaseChange battleStatePhaseChange;

    public virtual BossNode[] GetAttackPattern()
    {
        return bossAttackPattern;
    }

    public virtual BossPhase GetEndOfPatternPhaseChange()
    {
        if (!endOfPatternPhaseChange)
        {
            return null;
        }

        return endOfPatternPhaseChange;
    }

    public virtual HealthThresholdPhaseChange GetHealthPhaseChange()
    {
        if (!healthPhaseChange.GetNewPhase())
        {
            return null;
        }

        return healthPhaseChange;
    }

    public virtual BattleStatePhaseChange GetBattleStatePhaseChange()
    {
        if (!battleStatePhaseChange.GetBattleState())
        {
            return null;
        }

        return battleStatePhaseChange;
    }

    public virtual void InitialiseBossPhase(BossPhaseManager bossPhaseManager)
    {
        battleStatePhaseChange?.InitialiseBattleState(bossPhaseManager.transform);
    }

    public virtual void DeactivateBossPhase()
    {
        battleStatePhaseChange?.DeactivateBattleState();
    }

    public virtual void ResetPhase()
    {
        battleStatePhaseChange?.ResetBattleState();
    }
}
