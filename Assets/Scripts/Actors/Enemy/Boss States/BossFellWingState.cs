using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class BossFellWingState : BossState
{
    private int damage = 25;
    private int attackNumber = 1;
    private float damageWarningTime = 0.5f;
    private float telegraphDuration = 3f;
    private float timeBetweenSlashes = 1.5f;

    private const string chargeKey = "CB_FELL_CHARGE";
    private const string telegraphKey = "FELL_TELE";
    private const string attackKey = "FELL_WING";

    private MeleeAttack leftSlash;
    private MeleeAttack rightSlash;

    private ChangebringerFellWingPositioner wingPositioner;

    private List<int> wingStrikeDirection = new List<int>();

    public static EventHandler<int> OnWingTelegraph;

    public BossFellWingState(BossStateMachine stateMachine, int attackVersion = 0)
        : base(stateMachine)
    {
        leftSlash = new MeleeAttack(
            damage,
            DamageZoneType.box,
            new Vector2(50f, 50f),
            damageWarningTime,
            new Vector3(-50f, 0f, 0f),
            default,
            default,
            default,
            default,
            500f
        );

        rightSlash = leftSlash;
        rightSlash.attackPosition = new Vector3(50f, 0f, 0f);

        attackNumber = 1 + attackVersion;

        wingPositioner = stateMachine?.GetComponent<ChangebringerFellWingPositioner>();
    }

    public override void Enter()
    {
        BossCastBarUI.InitiateCastEvent(new CastInfo("Fell Wing", telegraphDuration));
        wingStrikeDirection = new List<int>();

        for (int i = 0; i < attackNumber; i++)
        {
            wingStrikeDirection.Add(Random.Range(0, 2));
        }

        //Perform wing telegraphs first

        bossStateMachine.StartCoroutine(PerformTelegraphs(attackNumber));
    }

    public override void Exit()
    {
        TryFinishState();
    }

    public override void Tick(float deltaTime) { }

    private void PlayTelegraph(int directionTelegraph)
    {
        Transform telegraphTransform = bossStateMachine.transform;

        if (wingPositioner)
        {
            if (directionTelegraph <= 0)
            {
                telegraphTransform = wingPositioner?.GetLeftWingTransform();
            }
            else
            {
                telegraphTransform = wingPositioner?.GetRightWingTransform();
            }
        }

        OnWingTelegraph?.Invoke(this, directionTelegraph);

        VFXManager.Instance.TryPlayVFX(telegraphKey, telegraphTransform);
    }

    private IEnumerator PerformTelegraphs(int attackNumber)
    {
        float timeBetweenTelegraphs = telegraphDuration / (attackNumber + 1);

        VFXManager.Instance.TryPlayVFX(chargeKey, bossStateMachine.transform);

        yield return new WaitForSeconds(timeBetweenTelegraphs);

        for (int i = 0; i < attackNumber; i++)
        {
            Debug.Log("Telegraph: " + wingStrikeDirection[i]);
            PlayTelegraph(wingStrikeDirection[i]);
            yield return new WaitForSeconds(timeBetweenTelegraphs);
        }

        bossStateMachine.StartCoroutine(PerformAttacks());
    }

    private IEnumerator PerformAttacks()
    {
        VFXManager.Instance.StopVFX(chargeKey);

        for (int i = 0; i < wingStrikeDirection.Count; i++)
        {
            MeleeAttack activeAttack;
            Transform attackTransform;

            if (wingStrikeDirection[i] <= 0)
            {
                activeAttack = leftSlash;
                attackTransform = wingPositioner?.GetLeftAttackTransform();
            }
            else
            {
                activeAttack = rightSlash;
                attackTransform = wingPositioner?.GetRightAttackTransform();
            }

            MeleeAttack[] attackHolder = { activeAttack };

            if (attackTransform == null)
            {
                attackTransform = bossStateMachine.transform;
            }

            bossStateMachine.GetMeleeAttacker().PerformMeleeAttacks(attackHolder, damageMult, null);

            VFXManager.Instance.TryPlayVFX(
                attackKey,
                attackTransform,
                activeAttack.zoneWarningTime
            );

            yield return new WaitForSeconds(timeBetweenSlashes);
        }

        stateMachine.SwitchState(null);
    }
}
