using UnityEngine;

public class BossPhaseManager : MonoBehaviour
{
    private int phaseTracker = 0;
    private BossPhase activePhase;

    [SerializeField]
    private BossPhase[] bossPhases;

    [SerializeField]
    private BossConditionalManager bossConditionalManager;

    public bool TryGetPhase(out BossPhase currentPhase)
    {
        currentPhase = null;
        if (phaseTracker >= bossPhases.Length)
        {
            return false;
        }

        currentPhase = bossPhases[phaseTracker];

        return true;
    }

    public void SwitchPhase(BossPhase newPhase)
    {
        if (activePhase == newPhase)
        {
            return;
        }

        activePhase?.DeactivateBossPhase();
        activePhase = newPhase;
        activePhase?.InitialiseBossPhase(this);
    }

    public void ResetCurrentPhase()
    {
        activePhase.ResetPhase();
        bossConditionalManager.ResetConditionals();
    }

    public void AdvancePhaseTracker()
    {
        phaseTracker++;
        bossConditionalManager.SaveConditionals();
    }

    public int GetCurrentPhaseIndex()
    {
        return phaseTracker;
    }

    public int GetTotalPhases()
    {
        return bossPhases.Length;
    }

    public BossConditionalManager GetConditionalManager()
    {
        return bossConditionalManager;
    }
}
