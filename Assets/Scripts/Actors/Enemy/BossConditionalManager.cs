using UnityEngine;

public class BossConditionalManager : MonoBehaviour
{
    [SerializeField]
    private BossConditional[] conditionals;

    public void SaveConditionals()
    {
        foreach (BossConditional conditional in conditionals)
        {
            conditional.SaveConditionalProgress();
        }
    }

    public void ResetConditionals()
    {
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
}
