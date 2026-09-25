using UnityEngine;

public abstract class BattleState : MonoBehaviour
{
    protected bool battleStateActive = false;

    public abstract bool ResolveBattleState();
    public abstract void ResetBattleState();
    public abstract BossPhase GetNewPhase();

    public virtual void ActivateListeners()
    {
        if (battleStateActive)
        {
            return;
        }

        battleStateActive = true;
    }

    public virtual void DeactivateListeners()
    {
        if (!battleStateActive)
        {
            return;
        }

        battleStateActive = false;
    }
}
