using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossElementalCardinalsState : BossState
{
    private int damage = 30;
    private int attackNumber = 1;
    private float damageWarningTime = 0.25f;
    private float telegraphDuration = 4f;
    private float timeBetweenStrikes = 0.75f;

    private const string chargeKey = "CB_FELL_CHARGE";
    private const string telegraphKey = "FELL_TELE";
    private const string attackKey = "FELL_WING";

    private MeleeAttack northSlash;
    private MeleeAttack westSlash;
    private MeleeAttack southSlash;
    private MeleeAttack eastSlash;

    private List<int> strikeDirection = new List<int>();

    public BossElementalCardinalsState(BossStateMachine stateMachine, int attackVersion = 0)
        : base(stateMachine)
    {
        attackNumber = attackVersion + 1;

        northSlash = new MeleeAttack(
            damage,
            DamageZoneType.circle,
            new Vector2(50f, 0.5f),
            damageWarningTime,
            Vector3.zero,
            0f,
            default,
            default,
            default,
            500f
        );

        westSlash = northSlash;
        westSlash.attackYRotation = 270f;

        eastSlash = northSlash;
        eastSlash.attackYRotation = 90f;

        southSlash = northSlash;
        southSlash.attackYRotation = 180f;
    }

    public override void Enter()
    {
        BossCastBarUI.InitiateCastEvent(
            new CastInfo("The Four Elemental Cardinals", telegraphDuration * attackNumber)
        );

        strikeDirection = new List<int> { Random.Range(0, 4) };

        if (attackNumber > 1)
        {
            GenerateAdditionalStrikeDirection();
        }

        //Perform direction telegraphs first;

        bossStateMachine.StartCoroutine(PerformTelegraphs(attackNumber));
    }

    public override void Exit()
    {
        VFXManager.Instance.StopVFX(chargeKey);
        TryFinishState();
    }

    public override void Tick(float deltaTime) { }

    private void GenerateAdditionalStrikeDirection()
    {
        int adjacentNumber = (int)
            AdditionalMath.Modulus(
                strikeDirection[0] + AdditionalMath.Remap(Random.Range(0, 2), 0f, 1f, -1f, 1f),
                4f
            );

        strikeDirection.Add(adjacentNumber);
    }

    private void PlayTelegraph(int directionTelegraph)
    {
        Transform telegraphTransform = bossStateMachine.transform;

        // Different voiceline / visual indicator for telegraphs

        // if (wingPositioner)
        // {
        //     if (directionTelegraph <= 0)
        //     {
        //         telegraphTransform = wingPositioner?.GetLeftWingTransform();
        //     }
        //     else
        //     {
        //         telegraphTransform = wingPositioner?.GetRightWingTransform();
        //     }
        // }

        //OnWingTelegraph?.Invoke(this, directionTelegraph);

        //VFXManager.Instance.TryPlayVFX(telegraphKey, telegraphTransform);
    }

    private void DebugTelegraph(int attackDirection)
    {
        switch (attackDirection)
        {
            case 0:
                Debug.Log("Northern Nature");
                break;

            case 1:
                Debug.Log("Eastern Embers");
                break;

            case 2:
                Debug.Log("Southern Seas");
                break;

            case 3:
                Debug.Log("Western Winds");
                break;

            default:
                Debug.Log("Northern Nature");
                break;
        }
    }

    private IEnumerator PerformTelegraphs(int attackNumber)
    {
        VFXManager.Instance.TryPlayVFX(chargeKey, bossStateMachine.transform);
        DebugTelegraph(strikeDirection[0]);

        yield return new WaitForSeconds(telegraphDuration);

        if (attackNumber > 1)
        {
            DebugTelegraph(strikeDirection[1]);

            yield return new WaitForSeconds(telegraphDuration);
        }

        bossStateMachine.StartCoroutine(PerformAttacks());
    }

    private IEnumerator PerformAttacks()
    {
        VFXManager.Instance.StopVFX(chargeKey);

        for (int i = 0; i < strikeDirection.Count; i++)
        {
            MeleeAttack activeAttack;
            Vector3 attackRotation = Vector3.zero;

            switch (strikeDirection[i])
            {
                case 0:
                    activeAttack = northSlash;
                    break;

                case 1:
                    activeAttack = eastSlash;
                    attackRotation.y = 90f;
                    break;

                case 2:
                    activeAttack = southSlash;
                    attackRotation.y = 180f;
                    break;

                case 3:
                    activeAttack = westSlash;
                    attackRotation.y = 270f;
                    break;

                default:
                    activeAttack = northSlash;
                    break;
            }

            MeleeAttack[] attackHolder = { activeAttack };

            bossStateMachine
                .GetMeleeAttacker()
                .PerformMeleeAttacks(attackHolder, damageMult, null, attackSignature);

            VFXManager.Instance.TryPlayVFX(
                attackKey,
                bossStateMachine.transform.position,
                Quaternion.Euler(attackRotation),
                activeAttack.zoneWarningTime
            );

            yield return new WaitForSeconds(timeBetweenStrikes);
        }

        stateMachine.SwitchState(null);
    }
}
