using UnityEngine;

public class BossStateMachine : StateMachine
{
    [SerializeField]
    protected Animator animator;

    [SerializeField]
    protected BossMeleeAttacker meleeAttacker;

    public Animator GetAnimator()
    {
        return animator;
    }

    public BossMeleeAttacker GetMeleeAttacker()
    {
        return meleeAttacker;
    }
}
