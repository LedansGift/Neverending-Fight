using UnityEngine;

public class BossRefractedReasoningState : BossState
{
    public BossRefractedReasoningState(BossStateMachine stateMachine)
        : base(stateMachine) { }

    public override void Enter() { }

    public override void Exit()
    {
        TryFinishState();
    }

    public override void Tick(float deltaTime) { }
}
