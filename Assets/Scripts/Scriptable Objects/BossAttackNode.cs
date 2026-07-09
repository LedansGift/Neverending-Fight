using System;
using UnityEngine;

public abstract class BossAttackNode : ScriptableObject
{
    [SerializeField]
    private string animationTrigger;

    [SerializeField]
    private string attackName = "DefaultName";

    [SerializeField]
    private BossAttackNode empoweredAttack;

    protected Action OnAttackFinished;
    public static EventHandler OnAttackFailCheck;
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

    public bool TryGetEmpoweredAttack(out BossAttackNode empoweredAttack)
    {
        empoweredAttack = null;
        if (this.empoweredAttack == null)
        {
            return false;
        }

        empoweredAttack = this.empoweredAttack;
        return true;
    }
}
