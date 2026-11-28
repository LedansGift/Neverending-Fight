using UnityEngine;

public class MagusDifferentialInterrupt : BossInterrupt
{
    private bool phaseStartShieldSave = false;
    private bool shieldActivated = false;

    public override void ResetInterrupt()
    {
        shieldActivated = phaseStartShieldSave;
    }

    public override bool ResolveInterrupt(out BossInterruptPhase outInterruptPhase)
    {
        //if shield not active and conditions met, return true with shield activate phase;
        outInterruptPhase = bossInterruptPhase;
        return false;
    }

    public override void SaveInterrupt()
    {
        phaseStartShieldSave = shieldActivated;
    }

    protected override void ActivateInterrupt()
    {
        //Record how much Champion has been using non-tome weapons
    }

    protected override void DeactivateInterrupt()
    {
        //Undo activate events
    }
}
