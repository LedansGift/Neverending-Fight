using UnityEngine;

public class BossStateMachine : StateMachine
{
    [SerializeField]
    protected Animator animator;

    [SerializeField]
    protected BossMeleeAttacker meleeAttacker;

    [SerializeField]
    protected BossMover mover;

    public Animator GetAnimator()
    {
        return animator;
    }

    public BossMeleeAttacker GetMeleeAttacker()
    {
        return meleeAttacker;
    }

    public BossMover GetMover()
    {
        return mover;
    }
}
