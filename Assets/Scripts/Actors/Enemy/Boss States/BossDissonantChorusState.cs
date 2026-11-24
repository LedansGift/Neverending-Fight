using System;
using System.Collections;
using UnityEngine;

public class BossDissonantChorusState : BossState
{
    private bool castActive = false;

    private int damage = 999;
    private float attackRadius = 100f;
    private float castTimer = 0f;
    private float castDuration = 10f;
    private float attackPreHitDelay = 0.5f;
    private float attackPostHitDelay = 2f;
    private const string chorusChargeKey = "CB_HCHARGE";
    private const string chorusAttackKey = "DISS_CHORUS";

    private MeleeAttack chorusAttack;
    private DamageZone chorusDamageZone;

    private BossCombatManager bossCombatManager;

    public BossDissonantChorusState(BossStateMachine stateMachine)
        : base(stateMachine)
    {
        chorusAttack = new MeleeAttack(
            damage,
            DamageZoneType.raidwide,
            new Vector2(attackRadius, 1f),
            castDuration,
            default,
            default,
            default,
            default,
            default,
            500f
        );

        bossCombatManager = stateMachine.GetComponent<BossCombatManager>();
    }

    public override void Enter()
    {
        //Start whirwind animation

        BossCastBarUI.InitiateCastEvent(new CastInfo("Dissonant Chorus", castDuration));

        bossStateMachine.GetBossHealth().OnTakeDamage += EvaluateHealthPhaseChange;

        chorusDamageZone = AttackTelegraphManager.Instance.StartAttack(
            bossStateMachine.transform,
            chorusAttack
        );

        castTimer = 0f;
        castActive = true;
    }

    public override void Exit()
    {
        bossStateMachine.GetBossHealth().OnTakeDamage -= EvaluateHealthPhaseChange;

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
        chorusDamageZone?.DeactivateZone();
        VFXManager.Instance.StopVFX(chorusChargeKey);

        BossCastBarUI.CancelCast();

        yield return new WaitForSeconds(attackPreHitDelay);

        bossStateMachine
            .GetMeleeAttacker()
            .PerformAttackUntelegraphed(chorusAttack, damageMult, attackSignature);

        VFXManager.Instance.TryPlayVFX(chorusAttackKey, bossStateMachine.transform);

        yield return new WaitForSeconds(attackPostHitDelay);

        stateMachine.SwitchState(null);
    }

    private void InterruptAttack()
    {
        chorusDamageZone?.DeactivateZone();
        VFXManager.Instance.StopVFX(chorusChargeKey);

        BossCastBarUI.CancelCast();

        stateMachine.SwitchState(null);
    }

    private void EvaluateHealthPhaseChange()
    {
        if (bossCombatManager.GetIsHealthThresholdPhaseChangeAvailable())
        {
            InterruptAttack();
        }
    }
}
