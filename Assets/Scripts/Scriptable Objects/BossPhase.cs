using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New Boss Phase", menuName = "Boss Phase", order = 0)]
public class BossPhase : ScriptableObject
{
    [SerializeField]
    protected BossNode[] bossAttackPattern;

    [SerializeField]
    protected HealthThresholdPhaseChange healthPhaseChange;

    public virtual BossNode[] GetAttackPattern()
    {
        return bossAttackPattern;
    }

    public virtual HealthThresholdPhaseChange GetHealthPhaseChange()
    {
        if (!healthPhaseChange.GetNewPhase())
        {
            return null;
        }

        return healthPhaseChange;
    }

    public virtual void InitialiseBossPhase(BossFormManager bossFormManager) { }
}
