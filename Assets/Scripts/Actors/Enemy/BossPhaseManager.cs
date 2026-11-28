using System.Collections.Generic;
using UnityEngine;

public class BossPhaseManager : MonoBehaviour
{
    private int phaseTracker = 0;
    private BossPhase activePhase;

    [SerializeField]
    private BossPhase[] phases;

    [SerializeField]
    private BossInterruptManager interruptManager;

    [SerializeField]
    private BossConditionalManager conditionalManager;

    public bool TryGetPhase(out BossPhase currentPhase)
    {
        currentPhase = null;
        if (phaseTracker >= phases.Length)
        {
            return false;
        }

        currentPhase = phases[phaseTracker];

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
        conditionalManager.ResetConditionals();
        interruptManager.ResetInterrupts();
    }

    public void AdvancePhaseTracker()
    {
        phaseTracker++;
        conditionalManager.SaveConditionals();
        interruptManager.SaveInterrupts();
    }

    public int GetCurrentPhaseIndex()
    {
        return phaseTracker;
    }

    public int GetTotalPhases()
    {
        return phases.Length;
    }

    public List<BossNode> GetInterruptNodes()
    {
        int[] activeInterrupts = activePhase.GetInterruptIndeces();

        List<BossNode> interruptNodes = new List<BossNode>();

        foreach (int interruptIndex in activeInterrupts)
        {
            if (
                interruptManager.ResolveInterrupt(
                    interruptIndex,
                    out BossInterruptPhase interruptPhase
                ) && (interruptPhase != null)
            )
            {
                interruptNodes.AddRange(interruptPhase.GetAttackPattern());
            }
        }

        return interruptNodes;
    }

    public BossConditionalManager GetConditionalManager()
    {
        return conditionalManager;
    }

    public void TogglePhaseManagerActive(bool toggle)
    {
        conditionalManager.ToggleBossActive(toggle);
        interruptManager.ToggleBossActive(toggle);
    }
}
