using System.Collections;
using UnityEngine;

public class BossDramaticLungeState : BossState
{
    private bool castActive = false;

    private int damage = 25;
    private float attackWidth = 2f;
    private float attackLength = 30f;
    private float castTimer = 0f;
    private float castDuration = 3f;
    private float attackPreHitDelay = 0.75f;
    private float attackPostHitDelay = 2f;

    private MeleeAttack lungeAttack;

    public BossDramaticLungeState(BossStateMachine stateMachine)
        : base(stateMachine)
    {
        lungeAttack = new MeleeAttack(
            damage,
            DamageZoneType.box,
            new Vector2(attackWidth, attackLength),
            attackPreHitDelay,
            new Vector3(0f, 0f, attackLength),
            0f,
            true,
            0f,
            true
        );
    }

    public override void Enter()
    {
        //Start lunge animation
        BossCastBarUI.InitiateCastEvent(new CastInfo("Dramatic Lunge", castDuration));

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
            bossStateMachine.StartCoroutine(PerformAttack());
            castActive = false;
        }
    }

    private IEnumerator PerformAttack()
    {
        // Vector3 lungeDirection = (
        //     PlayerIdentifier.PlayerTransform.position - bossStateMachine.transform.position
        // ).normalized;

        // lungeAttack.attackPosition = lungeDirection * attackLength;

        MeleeAttack[] lungerAttackHolder = { lungeAttack };

        bossStateMachine
            .GetMeleeAttacker()
            .PerformMeleeAttacks(lungerAttackHolder, damageMult, null);

        bossStateMachine.GetMover().CancelLockOn();

        yield return new WaitForSeconds(attackPreHitDelay);

        //KnockbackTest();

        yield return new WaitForSeconds(attackPostHitDelay);

        stateMachine.SwitchState(null);
    }

    private void KnockbackTest()
    {
        PlayerHealth playerHealth = PlayerIdentifier.PlayerTransform.GetComponent<PlayerHealth>();

        Vector3 knockbackDirection =
            playerHealth.transform.position - bossStateMachine.transform.position;

        playerHealth.Knockback(knockbackDirection.normalized, 500f);
    }
}
