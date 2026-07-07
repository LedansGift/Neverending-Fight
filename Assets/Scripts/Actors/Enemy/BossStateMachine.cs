using UnityEngine;

public class BossStateMachine : StateMachine
{
    [SerializeField]
    protected Animator animator;

    public Animator GetAnimator()
    {
        return animator;
    }
}
