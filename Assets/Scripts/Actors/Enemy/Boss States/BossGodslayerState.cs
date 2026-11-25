using System.Collections;
using UnityEngine;

public class BossGodslayerState : BossState
{
    private bool castActive = false;

    private int damage = 999;
    private float attackRadius = 100f;
    private float castTimer = 0f;
    private float castDuration = 20f;
    private float attackPreHitDelay = 0.5f;
    private float attackPostHitDelay = 2f;
    private const string chargeKey = "WIND_CHARGE";
    private const string godSlayerAttackKey = "WHIRLWIND";

    private MeleeAttack godSlayerAttack;
    private DamageZone godSlayerDamageZone;

    public BossGodslayerState(BossStateMachine stateMachine)
        : base(stateMachine)
    {
        godSlayerAttack = new MeleeAttack(
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
    }

    public override void Enter()
    {
        //Start whirwind animation

        BossCastBarUI.InitiateCastEvent(new CastInfo("Godslayer", castDuration));

        godSlayerDamageZone = AttackTelegraphManager.Instance.StartAttack(
            bossStateMachine.transform,
            godSlayerAttack
        );
        castTimer = 0f;
        castActive = true;
    }

    public override void Exit()
    {
        VFXManager.Instance.StopVFX(chargeKey);

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
        godSlayerDamageZone?.DeactivateZone();
        VFXManager.Instance.StopVFX(chargeKey);

        BossCastBarUI.CancelCast();

        yield return new WaitForSeconds(attackPreHitDelay);

        bossStateMachine
            .GetMeleeAttacker()
            .PerformAttackUntelegraphed(godSlayerAttack, damageMult, attackSignature);

        VFXManager.Instance.TryPlayVFX(godSlayerAttackKey, bossStateMachine.transform);

        yield return new WaitForSeconds(attackPostHitDelay);

        stateMachine.SwitchState(null);
    }
}
