using UnityEngine;

public class BossPhaseManager : MonoBehaviour
{
    private int phaseTracker = 0;

    [SerializeField]
    private BossPhase[] bossPhases;

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

    public void AdvancePhaseTracker()
    {
        phaseTracker++;
    }
}
