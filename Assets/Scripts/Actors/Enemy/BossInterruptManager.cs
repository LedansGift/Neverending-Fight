using UnityEngine;

public class BossInterruptManager : MonoBehaviour
{
    private bool bossActive = false;

    [SerializeField]
    private BossInterrupt[] bossInterrupts;

    public void SaveInterrupts()
    {
        if (!bossActive)
        {
            return;
        }

        foreach (BossInterrupt interrupt in bossInterrupts)
        {
            interrupt.SaveInterrupt();
        }
    }

    public void ResetInterrupts()
    {
        if (!bossActive)
        {
            return;
        }

        foreach (BossInterrupt interrupt in bossInterrupts)
        {
            interrupt.ResetInterrupt();
        }
    }

    public bool ResolveInterrupt(int interruptIndex, out BossInterruptPhase interruptPhase)
    {
        BossInterrupt interrupt = bossInterrupts?[interruptIndex];

        interruptPhase = null;

        if (!interrupt)
        {
            return false;
        }

        bool interruptResolved = interrupt.ResolveInterrupt(out interruptPhase);

        return interruptResolved;
    }

    public void ToggleBossActive(bool toggle)
    {
        bossActive = toggle;

        foreach (BossInterrupt interrupt in bossInterrupts)
        {
            interrupt.ToggleInterruptActive(bossActive);
        }
    }
}
