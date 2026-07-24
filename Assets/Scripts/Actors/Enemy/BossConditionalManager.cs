using UnityEngine;

public class BossConditionalManager : MonoBehaviour
{
    [SerializeField]
    private BossConditional[] conditionals;

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
