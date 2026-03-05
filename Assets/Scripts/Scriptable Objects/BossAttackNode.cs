using System;
using UnityEngine;

public abstract class BossAttackNode : ScriptableObject
{
    [SerializeField]
    private string animationTrigger;

    [SerializeField]
    private BossAttackNode empoweredAttack;

    protected Action OnAttackFinished;
    public static EventHandler<bool> OnAttackFailCheck;
    public abstract void PerformAttack(
        BossAttackManager attacker,
        Action OnAttackFinished,
        float damageMultiplier = 1f
    );

    public virtual void FinishAttack(object sender, bool attackFailed)
    {
        OnAttackFinished();
    }

    public int GetAnimationTrigger()
    {
        return Animator.StringToHash(animationTrigger);
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
