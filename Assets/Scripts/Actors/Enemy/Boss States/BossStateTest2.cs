using UnityEngine;

public class BossStateTest2 : BossState
{
    public BossStateTest2(BossStateMachine stateMachine)
        : base(stateMachine) { }

    public override void Enter()
    {
        Debug.Log("Test State 2 Enter");
    }

    public override void Exit()
    {
        Debug.Log("Test State 2 Exit");
    }

    public override void Tick(float deltaTime) { }
}
