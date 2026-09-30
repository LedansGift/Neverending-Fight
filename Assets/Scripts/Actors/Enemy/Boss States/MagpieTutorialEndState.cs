using UnityEngine;

public class MagpieTutorialEndState : BossState
{
    private Health bossHealth;

    public MagpieTutorialEndState(BossStateMachine stateMachine)
        : base(stateMachine)
    {
        bossHealth = stateMachine.GetComponent<Health>();
    }

    public override void Enter()
    {
        bossHealth.SetUnkillable(false);
        bossHealth.TakeDamage(999);
    }

    public override void Exit()
    {
        TryFinishState();
    }

    public override void Tick(float deltaTime) { }
}
