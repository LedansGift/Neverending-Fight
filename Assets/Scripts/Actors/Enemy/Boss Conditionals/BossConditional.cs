using UnityEngine;

public abstract class BossConditional : MonoBehaviour
{
    protected bool conditionalActive = false;
    public abstract int ResolveConditional();

    public abstract void SaveConditionalProgress();
    public abstract void ResetConditionalProgress();

    protected abstract void ActivateConditional();
    protected abstract void DeactivateConditional();

    public virtual void ToggleConditionalActive(bool toggle)
    {
        if (toggle == conditionalActive)
        {
            return;
        }

        conditionalActive = toggle;

        if (conditionalActive)
        {
            ActivateConditional();
        }
        else
        {
            DeactivateConditional();
        }
    }
}
