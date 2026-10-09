using UnityEngine;

public class BossWhirlingStoneState : BossState
{
    public BossWhirlingStoneState(BossStateMachine stateMachine)
        : base(stateMachine) { }

    public override void Enter() { }

    public override void Exit()
    {
        TryFinishState();
    }

    public override void Tick(float deltaTime) { }
}
