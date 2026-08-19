public class BossInescapablePinionsState : BossState
{
    private bool castActive = false;
    private float castTimer = 0f;
    private float castDuration = 1f;

    public BossInescapablePinionsState(BossStateMachine stateMachine)
        : base(stateMachine) { }

    public override void Enter()
    {
        castTimer = 0f;
        castActive = true;

        bossStateMachine.GetMover().LockOnTarget(PlayerIdentifier.PlayerTransform);
    }

    public override void Exit()
    {
        TryFinishState();
    }

    public override void Tick(float deltaTime)
    {
        if (!castActive)
        {
            return;
        }

        castTimer += deltaTime;

        if (castTimer >= castDuration)
        {
            bossStateMachine.GetMover().CancelLockOn();
            castActive = false;
            stateMachine.SwitchState(null);
        }
    }
}
