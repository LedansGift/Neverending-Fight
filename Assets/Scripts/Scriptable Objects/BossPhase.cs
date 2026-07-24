using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New Boss Phase", menuName = "Boss Phase", order = 0)]
public class BossPhase : ScriptableObject
{
    [SerializeField]
    protected BossAttackNode[] bossAttackPattern;

    [SerializeField]
    protected HealthThresholdPhaseChange healthPhaseChange;

    public virtual BossAttackNode[] GetAttackPattern()
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
