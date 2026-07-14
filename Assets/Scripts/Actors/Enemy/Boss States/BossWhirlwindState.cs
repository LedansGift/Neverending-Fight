using System;
using System.Collections;
using UnityEngine;

public class BossWhirlwindState : BossState
{
    private bool castActive = false;

    private int damage = 25;
    private float attackRadius = 50f;
    private float castTimer = 0f;
    private float castDuration = 6f;
    private float attackPreHitDelay = 0.5f;
    private float attackPostHitDelay = 2f;

    private MeleeAttack whirlwindAttack;
    private DamageZone whirlwindDamageZone;

    public BossWhirlwindState(BossStateMachine stateMachine)
        : base(stateMachine)
    {
        whirlwindAttack = new MeleeAttack(
            damage,
            DamageZoneType.circle,
            new Vector2(attackRadius, 1f),
            castDuration
        );
    }

    public override void Enter()
    {
        //Start whirwind animation

        BossCastBarUI.InitiateCastEvent(new CastInfo("Whirlwind", castDuration));
        PlayerGlaive.OnGlaiveSpecial += PlayerGlaive_OnGlaiveSpecial;
        whirlwindDamageZone = AttackTelegraphManager.Instance.StartAttack(
            bossStateMachine.transform,
            whirlwindAttack
        );
        castTimer = 0f;
        castActive = true;
    }

    public override void Exit()
    {
        PlayerGlaive.OnGlaiveSpecial -= PlayerGlaive_OnGlaiveSpecial;

        OnStateFinished();
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
        whirlwindDamageZone?.DeactivateZone();

        BossCastBarUI.CancelCast();

        yield return new WaitForSeconds(attackPreHitDelay);

        bossStateMachine.GetMeleeAttacker().PerformAttackUntelegraphed(whirlwindAttack, damageMult);

        KnockbackTest();

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

    private void PlayerGlaive_OnGlaiveSpecial(object sender, bool specialStart)
    {
        if (specialStart && castActive)
        {
            bossStateMachine.StartCoroutine(PerformAttack());
            castActive = false;
        }
    }
}
