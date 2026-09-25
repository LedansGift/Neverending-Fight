using UnityEngine;

public abstract class BossConditional : MonoBehaviour
{
    public abstract int ResolveConditional();

    public abstract void SaveConditionalProgress();
    public abstract void ResetConditionalProgress();
}
