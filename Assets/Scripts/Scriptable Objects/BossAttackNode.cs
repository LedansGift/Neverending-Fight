using System;
using UnityEngine;

public abstract class BossAttackNode : ScriptableObject
{
    protected Action OnAttackFinished;
    public abstract void PerformAttack(Transform attackTransform, Action OnAttackFinished);

    protected virtual void FinishAttack()
    {
        OnAttackFinished();
    }
}
