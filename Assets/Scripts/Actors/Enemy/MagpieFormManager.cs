using System;

public class MagpieFormManager : BossFormManager
{
    public static EventHandler<Action> OnTutorialPhaseChange;

    protected override void HandleBossDeath()
    {
        if (!bossActive)
        {
            return;
        }

        bossActive = false;
        bossAttackManager.PhaseEndCleanup();
        OnPhaseFinished?.Invoke();

        OnTutorialPhaseChange?.Invoke(this, InitiateDeathPhaseChange);
    }
}
