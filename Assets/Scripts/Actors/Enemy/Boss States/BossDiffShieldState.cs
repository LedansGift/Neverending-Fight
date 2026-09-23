using UnityEngine;

public class BossDiffShieldState : BossState
{
    private bool toggleShield;

    public BossDiffShieldState(BossStateMachine stateMachine, bool toggleShield)
        : base(stateMachine)
    {
        this.toggleShield = toggleShield;
    }

    public override void Enter()
    {
        if (stateMachine.TryGetComponent<BossMagusHealth>(out BossMagusHealth magusHealth))
        {
            magusHealth.ToggleShield(toggleShield);
        }

        //maybe wait for animation to play?

        stateMachine.SwitchState(null);
    }

    public override void Exit()
    {
        TryFinishState();
    }

    public override void Tick(float deltaTime) { }
}
