using System.Collections;
using UnityEngine;

public class BossStateTest1 : BossState
{
    public BossStateTest1(BossStateMachine stateMachine)
        : base(stateMachine) { }

    public override void Enter()
    {
        Debug.Log("Test State 1 Enter");

        stateMachine.StartCoroutine(TestCoroutine());
    }

    public override void Exit()
    {
        Debug.Log("Test State 1 Exit");

        if (OnStateFinished != null)
        {
            OnStateFinished();
        }
    }

    public override void Tick(float deltaTime) { }

    private IEnumerator TestCoroutine()
    {
        yield return new WaitForSeconds(1f);

        stateMachine.SwitchState(null);
    }
}
