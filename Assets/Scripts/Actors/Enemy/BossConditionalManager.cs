using UnityEngine;

public class BossConditionalManager : MonoBehaviour
{
    //Conditional Manager is used by Conditional Boss Phases. Allows for something like Magus Phase 2 to have multiple variants by resolving a specific Conditional
    //to have a particular style of phase based on player actions.

    private bool bossActive = false;

    [SerializeField]
    private BossConditional[] conditionals;

    public void SaveConditionals()
    {
        if (!bossActive)
        {
            return;
        }

        foreach (BossConditional conditional in conditionals)
        {
            conditional.SaveConditionalProgress();
        }
    }

    public void ResetConditionals()
    {
        if (!bossActive)
        {
            return;
        }

        foreach (BossConditional conditional in conditionals)
        {
            conditional.ResetConditionalProgress();
        }
    }

    public int ResolveConditional(int conditionalIndex)
    {
        BossConditional conditional = conditionals?[conditionalIndex];

        if (!conditional)
        {
            return 0;
        }

        return conditional.ResolveConditional();
    }

    public void ToggleBossActive(bool toggle)
    {
        bossActive = toggle;

        foreach (BossConditional bossConditional in conditionals)
        {
            bossConditional.ToggleConditionalActive(bossActive);
        }
    }
}
