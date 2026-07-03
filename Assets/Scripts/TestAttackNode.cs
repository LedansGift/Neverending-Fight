using System;

public enum AttackPatternType
{
    Idle,
    Move,
    MeleeAttack,
    RangedAttack
}

[Serializable]
public class TestAttackNode
{
    public AttackPatternType attackPatternType;

    public string animationTrigger;

    public TestAttackNode empoweredAttack;

    protected Action OnAttackFinished;
    public static EventHandler<bool> OnAttackFailCheck;

    // public abstract void PerformAttack(
    //     BossAttackManager attacker,
    //     Action OnAttackFinished,
    //     float damageMultiplier = 1f
    // );

    public virtual void FinishAttack(object sender, bool attackFailed)
    {
        if (OnAttackFinished != null)
        {
            OnAttackFinished();
        }
    }

    public int GetAnimationTrigger()
    {
        return UnityEngine.Animator.StringToHash(animationTrigger);
    }

    public bool TryGetEmpoweredAttack(out TestAttackNode empoweredAttack)
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
