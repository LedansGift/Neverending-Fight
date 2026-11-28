using UnityEngine;

public abstract class BossInterrupt : MonoBehaviour
{
    protected bool interruptActive = false;

    [SerializeField]
    protected BossInterruptPhase bossInterruptPhase;

    public abstract bool ResolveInterrupt(out BossInterruptPhase outInterruptPhase);

    public abstract void SaveInterrupt();
    public abstract void ResetInterrupt();

    protected abstract void ActivateInterrupt();
    protected abstract void DeactivateInterrupt();

    public virtual void ToggleInterruptActive(bool toggle)
    {
        if (toggle == interruptActive)
        {
            return;
        }

        interruptActive = toggle;

        if (interruptActive)
        {
            ActivateInterrupt();
        }
        else
        {
            DeactivateInterrupt();
        }
    }
}
