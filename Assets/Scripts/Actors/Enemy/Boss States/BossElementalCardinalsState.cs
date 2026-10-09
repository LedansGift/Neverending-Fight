using UnityEngine;

public class BossElementalCardinalsState : BossState
{
    private int attackNumber = 1;

    public BossElementalCardinalsState(BossStateMachine stateMachine, int attackVersion = 0)
        : base(stateMachine)
    {
        attackNumber = attackVersion + 1;
    }

    public override void Enter() { }

    public override void Exit()
    {
        TryFinishState();
    }

    public override void Tick(float deltaTime) { }
}
