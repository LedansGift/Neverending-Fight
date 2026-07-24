using System;
using UnityEngine;

[Serializable]
public class HealthThresholdPhaseChange
{
    [Range(0f, 100f)]
    [SerializeField]
    private float healthPercentageThreshold;

    [SerializeField]
    private BossPhase bossPhase;

    public float GetHealthThreshold()
    {
        return healthPercentageThreshold;
    }

    public BossPhase GetNewPhase()
    {
        return bossPhase;
    }
}
