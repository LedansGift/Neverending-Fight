using System;
using UnityEngine;

public abstract class BossNode : ScriptableObject
{
    [SerializeField]
    private string animationTrigger;

    [SerializeField]
    private string attackName = "DefaultName";
    protected Action OnAttackFinished;

    public abstract void PerformAttack(
        BossAttackManager attacker,
        Action OnAttackFinished,
        float damageMultiplier = 1f
    );

    public virtual void FinishAttack()
    {
        if (OnAttackFinished != null)
        {
            OnAttackFinished();
        }
    }

    public int GetAnimationTrigger()
    {
        return Animator.StringToHash(animationTrigger);
    }

    public string GetAttackName()
    {
        return attackName;
    }

    public virtual bool GetIsAttackNode()
    {
        return false;
    }
}
